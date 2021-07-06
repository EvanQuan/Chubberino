using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chubberino.Common.Extensions;
using Chubberino.Common.Services;
using Chubberino.Common.ValueObjects;
using Chubberino.Database.Contexts;
using Chubberino.Infrastructure.Credentials;
using Monad;
using TwitchLib.Client.Exceptions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Infrastructure.Client.TwitchClients
{
    public sealed class TwitchClientManager : ITwitchClientManager
    {
        public const Int32 MaxMessageLength = 500;

        public event EventHandler<OnTwitchClientRefreshedArgs> OnTwitchClientRefreshed;

        public String PrimaryChannelName { get; set; }

        private String PreviousMessage { get; set; }

        public Boolean IsBot { get; private set; }

        public ITwitchClient Client { get; private set; }

        private IApplicationContextFactory ContextFactory { get; }

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

        public LowercaseString Name { get; private set; }

        public TwitchClientManager(
            IApplicationContextFactory contextFactory,
            ITwitchClientFactory factory,
            ICredentialsManager credentialsManager,
            ISpinWaitService spinWaitService,
            IThreadService threadService,
            IDateTimeService dateTime,
            TextWriter writer)
        {
            ContextFactory = contextFactory;
            Factory = factory;
            CredentialsManager = credentialsManager;
            SpinWaitService = spinWaitService;
            ThreadService = threadService;
            DateTime = dateTime;
            Writer = writer;
            LastLowPriorityMessageSent = new();
        }

        public LoginCredentials TryInitializeTwitchClient(
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

            if (credentials is null && !CredentialsManager.TryLoginAsNewUser())
            {
                return null;
            }

            ConnectionCredentials = credentials.ConnectionCredentials;
            bot.LoginCredentials = credentials;
            Name = LowercaseString.From(credentials.ConnectionCredentials.TwitchUsername);
            IsBot = credentials.IsBot;

            if (PrimaryChannelName is null)
            {
                PrimaryChannelName = CredentialsManager.ApplicationCredentials.InitialTwitchPrimaryChannelName;
            }

            RefreshTwitchClient(bot);

            return credentials;
        }

        private void RefreshTwitchClient(IBot bot)
        {
            var oldClient = Client;

            OptionResult<ITwitchClient> optionOldClient = oldClient is null ? Option.Nothing<ITwitchClient>().Invoke() : oldClient.ToOption();

            Client = Factory.CreateClient(CurrentClientOptions);

            Client.Initialize(ConnectionCredentials, PrimaryChannelName);

            Client.OnJoinedChannel += (_, e) => { Writer.WriteLine($"Joined channel {e.Channel}"); };
            Client.OnConnected += (_, _) => { bot.State = BotState.ShouldContinue; };
            Client.OnConnectionError += (_, e) =>
            {
                Writer.WriteLine($"!! Connection Error!! {e.Error.Message}");

                bot.State = BotState.ShouldRestart;
            };

            OnTwitchClientRefreshedArgs = new OnTwitchClientRefreshedArgs(optionOldClient, Client);
        }

        public Boolean TryJoinInitialChannels(IReadOnlyList<JoinedChannel> previouslyJoinedChannels = null)
        {
            Boolean channelJoined = EnsureJoinedToChannel(PrimaryChannelName);

            if (!channelJoined) { return false; }

            using var context = ContextFactory.GetContext();

            if (IsBot)
            {
                foreach (var channel in context.StartupChannels)
                {
                    Client.JoinChannel(channel.DisplayName);
                }
            }

            if (previouslyJoinedChannels is not null && previouslyJoinedChannels.Any())
            {
                foreach (var channelName in previouslyJoinedChannels.Select(x => x.Channel))
                {
                    Writer.WriteLine("Connecting to " + channelName);
                    channelJoined = EnsureJoinedToChannel(channelName);
                    if (!channelJoined) { return false; }
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

            if (timeSinceLastMessage >= TimeSpan.FromSeconds(2))
            {
                SendMessageDirectly(channelName, message);
                LastLowPriorityMessageSent.AddOrUpdate(channelName, DateTime.Now, (_, _) => DateTime.Now);
            }
        }

        private void AddMessageToQueue(String channelName, String message)
        {
            throw new NotImplementedException();
        }

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
}
