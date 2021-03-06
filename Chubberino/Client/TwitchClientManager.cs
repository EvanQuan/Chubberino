﻿using Chubberino.Client.Credentials;
using Chubberino.Client.Services;
using Chubberino.Client.Threading;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Exceptions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    public sealed class TwitchClientManager : ITwitchClientManager
    {
        public const Int32 MaxMessageLength = 500;

        public String PrimaryChannelName { get; set; }

        private String PreviousMessage { get; set; }

        public Boolean IsBot { get; private set; }

        public ITwitchClient Client { get; private set; }

        public IApplicationContextFactory ContextFactory { get; }

        private ITwitchClientFactory Factory { get; }

        private ICredentialsManager CredentialsManager { get; }
        private ISpinWait SpinWait { get; }

        public IDateTimeService DateTime { get; }

        public IConsole Console { get; }

        private IClientOptions CurrentClientOptions { get; set; }

        private ConnectionCredentials ConnectionCredentials { get; set; }

        private ConcurrentDictionary<String, DateTime> LastLowPriorityMessageSent { get; }

        public TwitchClientManager(
            IApplicationContextFactory contextFactory,
            ITwitchClientFactory factory,
            ICredentialsManager credentialsManager,
            ISpinWait spinWait,
            IDateTimeService dateTime,
            IConsole console)
        {
            ContextFactory = contextFactory;
            Factory = factory;
            CredentialsManager = credentialsManager;
            SpinWait = spinWait;
            DateTime = dateTime;
            Console = console;
            LastLowPriorityMessageSent = new ConcurrentDictionary<String, DateTime>();
        }

        public LoginCredentials TryInitialize(
            IBot bot,
            IClientOptions clientOptions = null,
            LoginCredentials credentials = null)
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

            if (credentials == null)
            {
                if (!CredentialsManager.TryGetCredentials(out credentials))
                {
                    return null;
                }
            }

            ConnectionCredentials = credentials.ConnectionCredentials;
            bot.Name = credentials.ConnectionCredentials.TwitchUsername;
            bot.LoginCredentials = credentials;
            IsBot = credentials.IsBot;

            if (PrimaryChannelName == null)
            {
                PrimaryChannelName = CredentialsManager.ApplicationCredentials.InitialTwitchPrimaryChannelName;
            }

            Client = Factory.GetClient(CurrentClientOptions);

            Client.Initialize(ConnectionCredentials, PrimaryChannelName);

            Client.OnConnected += (_, _) => { bot.State = BotState.ShouldContinue; };
            Client.OnConnectionError += (_, e) =>
            {

                Console.WriteLine($"!! Connection Error!! {e.Error.Message}");

                bot.State = BotState.ShouldRestart;
            };

            return credentials;
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
