using Autofac;
using Chubberino.Client.Abstractions;
using Chubberino.Client.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    public sealed class Bot : IBot
    {
        private IClientOptions CurrentClientOptions { get; set; }

        public BotState State { get; private set; }

        public IExtendedClient TwitchClient { get; set; }

        private ConnectionCredentials Credentials { get; }

        private ICommandRepository Commands { get; set; }

        private TextWriter Console { get; set; }

        public ILifetimeScope Scope { get; set; }

        /// <summary>
        /// Current channel joined.
        /// </summary>
        public String ChannelName { get; set; }

        /// <summary>
        /// 100 messages in 30 seconds ~1 message per 0.3 seconds.
        /// </summary>
        public IClientOptions ModeratorClientOptions { get; }

        /// <summary>
        /// 20 messages in 30 seconds ~1 message per 1.5 second
        /// </summary>
        public IClientOptions RegularClientOptions { get; }

        /// <summary>
        /// Is the bot a broadcaster/moderator/VIP?
        /// </summary>
        public Boolean IsModerator { get; set; }

        private IExtendedClientFactory ClientFactory { get; }

        public ISpinWait SpinWait { get; }

        public Bot(
            TextWriter console,
            ICommandRepository commands,
            ConnectionCredentials credentials,
            IClientOptions moderatorOptions,
            IClientOptions regularOptions,
            IExtendedClientFactory clientFactory,
            ISpinWait spinWait,
            String channelName)
        {
            Credentials = credentials;
            Console = console;
            Commands = commands;
            ModeratorClientOptions = moderatorOptions;
            RegularClientOptions = regularOptions;
            ClientFactory = clientFactory;
            SpinWait = spinWait;
            ChannelName = channelName;
            InitializeTwitchClientAndSpooler(regularOptions);
        }

        private void InitializeTwitchClientAndSpooler(IClientOptions clientOptions)
        {
            CurrentClientOptions = clientOptions;

            TwitchClient = ClientFactory.GetClient(clientOptions);

            TwitchClient.Initialize(Credentials, ChannelName);

            TwitchClient.OnConnected += Client_OnConnected;
            TwitchClient.OnConnectionError += Client_OnConnectionError;
            TwitchClient.OnUserTimedout += Client_OnUserTimedout;
        }

        public Boolean Start()
        {
            Console.WriteLine("Connecting to " + ChannelName);
            Boolean channelJoined = TwitchClient.EnsureJoinedToChannel(ChannelName);

            if (!channelJoined) { return false; }

            Boolean channelNameUpdated = SpinWait.SpinUntil(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                return !String.IsNullOrWhiteSpace(ChannelName);
            },
            TimeSpan.FromSeconds(5));

            return channelNameUpdated;

        }

        public String GetPrompt()
        {
            return Environment.NewLine + Environment.NewLine + Commands.GetStatus() + Environment.NewLine
                + $"[{(IsModerator ? "Mod" : "Normal" )} {ChannelName}]> ";
        }

        private void Client_OnUserTimedout(Object sender, OnUserTimedoutArgs e)
        {
        }

        private void Client_OnConnectionError(Object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"!! Connection Error!! {e.Error.Message}");
            Refresh(CurrentClientOptions);
            Console.WriteLine("!! Refreshed");
        }

        public void Refresh(IClientOptions clientOptions)
        {
            InitializeTwitchClientAndSpooler(clientOptions);
            Commands.RefreshAll(TwitchClient);
            Start();
        }

        private void Client_OnConnected(Object sender, OnConnectedArgs e)
        {
            State = BotState.ShouldContinue;
        }

        public void ReadCommand(String command)
        {
            String[] arguments = command.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (arguments.Length == 0) { return; }

            String commandName = arguments[0].ToLower();

            switch (commandName)
            {
                case "quit":
                    State = BotState.ShouldStop;
                    break;

                default:
                    Commands.Execute(commandName, arguments.Skip(1));
                    break;
            }
        }

        public void Dispose()
        {
            if (TwitchClient.IsConnected)
            {
                TwitchClient.Disconnect();
            }
        }
    }
}
