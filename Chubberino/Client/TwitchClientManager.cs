using Chubberino.Client.Abstractions;
using Chubberino.Client.Credentials;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    public sealed class TwitchClientManager : ITwitchClientManager
    {
        public IExtendedClient Client { get; private set; }

        public String PrimaryChannelName { get; set; }
        public IApplicationContext Context { get; }
        private IExtendedClientFactory Factory { get; }

        private ICredentialsManager CredentialsManager { get; }

        private ApplicationCredentials ApplicationCredentials { get; }
        public IConsole Console { get; }
        private IClientOptions CurrentClientOptions { get; set; }

        private ConnectionCredentials ConnectionCredentials { get; set; }

        public TwitchClientManager(
            IApplicationContext context,
            IExtendedClientFactory factory,
            ICredentialsManager credentialsManager,
            ApplicationCredentials applicationCredentials,
            IConsole console)
        {
            Context = context;
            Factory = factory;
            CredentialsManager = credentialsManager;
            ApplicationCredentials = applicationCredentials;
            Console = console;
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
                if (CredentialsManager.TryGetConnectionCredentials(out var credentials))
                {
                    ConnectionCredentials = credentials;
                    bot.Name = credentials.TwitchUsername;
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

        public void SpoolMessage(String message)
        {
            Client.SpoolMessage(PrimaryChannelName, message);
        }

        private void Client_OnConnectionError(Object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"!! Connection Error!! {e.Error.Message}");
            Console.WriteLine("!! Refreshed");
        }

        public Boolean TryJoinInitialChannels(IReadOnlyList<JoinedChannel> previouslyJoinedChannels = null)
        {
            Boolean channelJoined = Client.EnsureJoinedToChannel(PrimaryChannelName);

            if (!channelJoined) { return false; }

            foreach (var channel in Context.StartupChannels)
            {
                Client.JoinChannel(channel.DisplayName);
            }

            if (previouslyJoinedChannels != null && previouslyJoinedChannels.Any())
            {
                foreach (var channel in previouslyJoinedChannels)
                {
                    var channelName = channel.Channel;

                    Console.WriteLine("Connecting to " + channelName);
                    channelJoined = Client.EnsureJoinedToChannel(channelName);

                    if (!channelJoined) { return false; }
                }
            }

            return true;
        }
    }
}
