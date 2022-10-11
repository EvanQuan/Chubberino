using System.Collections.Concurrent;
using System.IO;
using Chubberino.Common.Extensions;
using Chubberino.Common.Services;
using Chubberino.Common.ValueObjects;
using Chubberino.Infrastructure.Configurations;
using Chubberino.Infrastructure.Credentials;
using LanguageExt.SomeHelp;
using TwitchLib.Client.Exceptions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Infrastructure.Client.TwitchClients;

public sealed class TwitchClientManager : ITwitchClientManager
{
    public const Int32 MaxMessageLength = 500;

    public event EventHandler<OnTwitchClientRefreshedArgs> OnTwitchClientRefreshed;

    public String PrimaryChannelName { get; set; }

    private String PreviousMessage { get; set; }

    public Boolean IsBot { get; private set; }

    public ITwitchClient Client { get; private set; }

    private IConfig Config { get; }

    private ITwitchClientFactory Factory { get; }


    private ISpinWaitService SpinWaitService { get; }

    private IThreadService ThreadService { get; }

    private IDateTimeService DateTime { get; }

    private IClientOptions CurrentClientOptions { get; set; }

    private ConnectionCredentials ConnectionCredentials { get; set; }

    private ConcurrentDictionary<String, DateTime> LastLowPriorityMessageSent { get; }

    private OnTwitchClientRefreshedArgs OnTwitchClientRefreshedArgs { get; set; }

    private ICredentialsManager CredentialsManager { get; }

    private TextWriter Writer { get; }

    public Name Name { get; private set; }

    public TwitchClientManager(
        IConfig config,
        ITwitchClientFactory factory,
        ICredentialsManager credentialsManager,
        ISpinWaitService spinWaitService,
        IThreadService threadService,
        IDateTimeService dateTime,
        TextWriter writer)
    {
        Config = config;
        Factory = factory;
        CredentialsManager = credentialsManager;
        SpinWaitService = spinWaitService;
        ThreadService = threadService;
        DateTime = dateTime;
        Writer = writer;
        LastLowPriorityMessageSent = new();
    }

    public Option<LoginCredentials> TryInitializeTwitchClient(
        IBot bot,
        IClientOptions clientOptions = null,
        LoginCredentials credentials = null)
    {
        // We need to get all the channel that the old client was connected to,
        // so we can rejoin those channels on the new client.
        //var previouslyJoinedChannels = Client == null
        //    ? Array.Empty<JoinedChannel>()
        //    : Client.JoinedChannels.ToArray();

        if (clientOptions is not null)
        {
            CurrentClientOptions = clientOptions;
        }
        var maybeUpdatedCredentials = CredentialsManager.TryUpdateLoginCredentials(credentials);
        return maybeUpdatedCredentials
            .Some(updatedCredentials =>
            {
                ConnectionCredentials = updatedCredentials.ConnectionCredentials;
                bot.LoginCredentials = updatedCredentials;
                Name = Name.From(updatedCredentials.ConnectionCredentials.TwitchUsername);
                IsBot = updatedCredentials.IsBot;

                PrimaryChannelName ??= CredentialsManager.ApplicationCredentials.InitialTwitchPrimaryChannelName;

                RefreshTwitchClient(bot);

                return Option<LoginCredentials>.Some(updatedCredentials);
            })
            .None(() =>
            {
                Writer.WriteLine("Failed to update login credentials");
                return Option<LoginCredentials>.None;
            });
    }

    private void RefreshTwitchClient(IBot bot)
    {
        var oldClient = Client;

        Option<ITwitchClient> optionOldClient = oldClient is null ? Option<ITwitchClient>.None : oldClient.ToSome();

        Client = Factory.CreateClient(CurrentClientOptions);

        Client.Initialize(ConnectionCredentials, PrimaryChannelName);

        Client.OnJoinedChannel += (_, e) => { Writer.WriteLine($"Joined channel {e.Channel}"); };
        Client.OnConnected += (_, _) => { bot.State = BotState.ShouldContinue; };
        Client.OnConnectionError += (_, e) =>
        {
            Writer.WriteLine($"!! Connection Error!! {e.Error.Message}");

            bot.Refresh(CurrentClientOptions);
        };
        Client.OnDisconnected += (_, e) =>
        {
            Writer.WriteLine($"!! Disconnected Error!! {e}");
            bot.Refresh(CurrentClientOptions);
        };


        OnTwitchClientRefreshedArgs = new OnTwitchClientRefreshedArgs(optionOldClient, Client);
    }

    public Boolean TryJoinInitialChannels(IReadOnlyList<JoinedChannel> previouslyJoinedChannels = null)
    {
        Boolean primaryChannelJoined = EnsureJoinedToChannel(PrimaryChannelName);

        if (!primaryChannelJoined) { return false; }

        if (IsBot)
        {
            foreach (var channel in Config.StartupChannelDisplayNames)
            {
                Client.JoinChannel(channel);
            }
        }

        if (previouslyJoinedChannels is not null && previouslyJoinedChannels.Any())
        {
            foreach (var channelName in previouslyJoinedChannels.Select(x => x.Channel))
            {
                Writer.WriteLine("Connecting to " + channelName);
                Boolean otherChannelJoined = EnsureJoinedToChannel(channelName);
                if (!otherChannelJoined) { return false; }
            }
        }

        OnTwitchClientRefreshed?.Invoke(this, OnTwitchClientRefreshedArgs);

        return true;
    }


    public Boolean EnsureJoinedToChannel(String channelName)
    {
        Boolean isConnected = SpinWaitService.SpinUntil(() =>
        {
            if (!Client.IsConnected)
            {
                Client.Connect();
                ThreadService.Sleep(TimeSpan.FromSeconds(1));
                return Client.IsConnected;
            }
            return true;

        },
        TimeSpan.FromSeconds(10));

        if (!isConnected) { return false; }

        Boolean isJoined = SpinWaitService.SpinUntil(() =>
        {
            Client.JoinChannel(channelName);
            return Client.JoinedChannels.Any(x => x.Channel.Equals(channelName, StringComparison.OrdinalIgnoreCase));
        },
        TimeSpan.FromSeconds(10));

        return isJoined;
    }

    public void SpoolMessage(String channelName, String message, Priority priority = Priority.Medium)
    {
        var segments = message.SplitByLengthOnWord(MaxMessageLength);

        foreach (var segment in segments)
        {
            switch (priority)
            {
                case Priority.Low:
                    SendMessageConditionally(channelName, segment);
                    break;
                case Priority.Medium:
                    SendMessageDirectly(channelName, segment);
                    break;
                case Priority.High:
                    AddMessageToQueue(channelName, segment);
                    break;
            }
        }
    }

    private void SendMessageConditionally(String channelName, String message)
    {
        DateTime lastSent = LastLowPriorityMessageSent.GetOrAdd(channelName, DateTime.MinValue);

        TimeSpan timeSinceLastMessage = DateTime.Now - lastSent;

        if (timeSinceLastMessage >= TimeSpan.FromSeconds(3))
        {
            SendMessageDirectly(channelName, message);
            LastLowPriorityMessageSent.AddOrUpdate(channelName, DateTime.Now, (_, _) => DateTime.Now);
        }
    }

    private void AddMessageToQueue(String channelName, String message)
        => throw new NotImplementedException();

    private void SendMessageDirectly(String channelName, String message)
    {
        if (message == PreviousMessage)
        {
            message += Data.SpaceWithInvisibleCharacter;
        }

        Boolean messageSent = false;
        do
        {
            try
            {
                Client.SendMessage(channelName, message);
                PreviousMessage = message;
                messageSent = true;
            }
            catch (BadStateException e)
            {
                Writer.WriteLine("ERROR: Failed to send message");
                Writer.WriteLine(e.Message);

                Boolean connected;
                do
                {
                    Writer.WriteLine("Reconnecting");
                    connected = EnsureJoinedToChannel(channelName);
                }
                while (!connected);
            }
        }
        while (!messageSent);
    }
}
