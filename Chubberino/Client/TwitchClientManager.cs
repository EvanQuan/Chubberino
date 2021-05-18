using Chubberino.Client.Credentials;
using Chubberino.Client.Services;
using Chubberino.Client.Threading;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Exceptions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    public sealed class TwitchClientManager : ITwitchClientManager
    {
        public String PrimaryChannelName { get; set; }

        private String PreviousMessage { get; set; }

        public Boolean IsBot { get; private set; }

        public ITwitchClient Client { get; private set; }

        public IApplicationContext Context { get; }

        private ITwitchClientFactory Factory { get; }

        private ICredentialsManager CredentialsManager { get; }
        private ISpinWait SpinWait { get; }
        public IDateTimeService DateTime { get; }
        private ApplicationCredentials ApplicationCredentials { get; }

        public IConsole Console { get; }

        private IClientOptions CurrentClientOptions { get; set; }

        private ConnectionCredentials ConnectionCredentials { get; set; }

        private ConcurrentDictionary<String, DateTime> LastLowPriorityMessageSent { get; }

        public TwitchClientManager(
            IApplicationContext context,
            ITwitchClientFactory factory,
            ICredentialsManager credentialsManager,
            ISpinWait spinWait,
            IDateTimeService dateTime,
            ApplicationCredentials applicationCredentials,
            IConsole console)
        {
            Context = context;
            Factory = factory;
            CredentialsManager = credentialsManager;
            SpinWait = spinWait;
            DateTime = dateTime;
            ApplicationCredentials = applicationCredentials;
            Console = console;
            LastLowPriorityMessageSent = new ConcurrentDictionary<String, DateTime>();
        }

        public Boolean TryInitialize(
            IBot bot,
            IClientOptions clientOptions = null,
            Boolean askForCredentials = true)
        {
            // We need to get all the channel that the old client was connected to,
            // so we can rejoin those channels on the new client.
            var previouslyJoinedChannels = Client == null 
                ? Array.Empty<JoinedChannel>()
                : Client.JoinedChannels.ToArray();

            if (clientOptions != null)
            {
                CurrentClientOptions = clientOptions;
            }

            if (askForCredentials)
            {
                if (CredentialsManager.TryGetCredentials(out var credentials))
                {
                    ConnectionCredentials = credentials.ConnectionCredentials;
                    bot.Name = credentials.ConnectionCredentials.TwitchUsername;
                    IsBot = credentials.IsBot;
                }
                else
                {
                    return false;
                }
            }

            if (PrimaryChannelName == null)
            {
                PrimaryChannelName = ApplicationCredentials.InitialTwitchPrimaryChannelName;
            }

            Client = Factory.GetClient(CurrentClientOptions);

            Client.Initialize(ConnectionCredentials, PrimaryChannelName);

            Client.OnConnectionError += Client_OnConnectionError;
            Client.OnConnected += (_, _) => { bot.State = BotState.ShouldContinue; };

            return true;
        }

        private void Client_OnConnectionError(Object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"!! Connection Error!! {e.Error.Message}");
            Console.WriteLine("!! Refreshed");
        }

        public Boolean TryJoinInitialChannels(IReadOnlyList<JoinedChannel> previouslyJoinedChannels = null)
        {
            Boolean channelJoined = EnsureJoinedToChannel(PrimaryChannelName);

            if (!channelJoined) { return false; }

            if (IsBot)
            {
                foreach (var channel in Context.StartupChannels)
                {
                    Client.JoinChannel(channel.DisplayName);
                }
            }

            if (previouslyJoinedChannels != null && previouslyJoinedChannels.Any())
            {
                foreach (var channel in previouslyJoinedChannels)
                {
                    var channelName = channel.Channel;

                    Console.WriteLine("Connecting to " + channelName);
                    channelJoined = EnsureJoinedToChannel(channelName);

                    if (!channelJoined) { return false; }
                }
            }

            return true;
        }


        public Boolean EnsureJoinedToChannel(String channelName)
        {
            Boolean isConnected = SpinWait.SpinUntil(() =>
            {
                if (!Client.IsConnected)
                {
                    Client.Connect();
                    SpinWait.Sleep(TimeSpan.FromSeconds(1));
                    return Client.IsConnected;
                }
                return true;

            },
            TimeSpan.FromSeconds(10));

            if (!isConnected) { return false; }

            Boolean isJoined = SpinWait.SpinUntil(() =>
            {
                Client.JoinChannel(channelName);
                return Client.JoinedChannels.Any(x => x.Channel.Equals(channelName, StringComparison.OrdinalIgnoreCase));
            },
            TimeSpan.FromSeconds(10));

            return isJoined;
        }

        public void SpoolMessage(String channelName, String message, Priority priority = Priority.Medium)
        {
            switch (priority)
            {
                case Priority.Low:
                    SendMessageConditionally(channelName, message);
                    break;
                case Priority.Medium:
                    SendMessageDirectly(channelName, message);
                    break;
                case Priority.High:
                    AddMessageToQueue(channelName, message);
                    break;
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
                message += ' ' + Data.InvisibleCharacter;
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
                    Console.WriteLine("ERROR: Failed to send message");
                    Console.WriteLine(e.Message);

                    Boolean connected;
                    do
                    {
                        Console.WriteLine("Reconnecting");
                        connected = EnsureJoinedToChannel(channelName);
                    }
                    while (!connected);
                }
            }
            while (!messageSent);
        }
    }
}
