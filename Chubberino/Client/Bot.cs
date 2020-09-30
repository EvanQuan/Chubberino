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
    internal sealed class Bot : IBot
    {
        private IClientOptions CurrentClientOptions { get; set; }

        public BotState State { get; private set; }

        public IExtendedClient TwitchClient { get; set; }

        private ConnectionCredentials Credentials { get; }

        private ICommandRepository Commands { get; set; }

        private TextWriter Console { get; set; }

        public ILifetimeScope Scope { get; set; }

        private BotInfo BotInfo { get; }

        private IExtendedClientFactory ClientFactory { get; }

        public Bot(
            TextWriter console,
            ICommandRepository commands,
            ConnectionCredentials credentials,
            BotInfo botInfo,
            IExtendedClientFactory clientFactory)
        {
            Credentials = credentials;
            Console = console;
            Commands = commands;
            BotInfo = botInfo;
            ClientFactory = clientFactory;
            InitializeTwitchClientAndSpooler(BotInfo.RegularClientOptions);
        }

        private void InitializeTwitchClientAndSpooler(IClientOptions clientOptions)
        {
            CurrentClientOptions = clientOptions;

            TwitchClient = ClientFactory.GetClient(clientOptions);

            TwitchClient.Initialize(Credentials, BotInfo.ChannelName);

            TwitchClient.OnConnected += Client_OnConnected;
            TwitchClient.OnConnectionError += Client_OnConnectionError;
            TwitchClient.OnUserTimedout += Client_OnUserTimedout;
        }

        public Boolean Start()
        {
            Console.WriteLine("Connecting to " + BotInfo.ChannelName);
            TwitchClient.EnsureJoinedToChannel(BotInfo.ChannelName);

            Boolean channelJoined = SpinWait.SpinUntil(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                return !String.IsNullOrWhiteSpace(BotInfo.ChannelName);
            },
            TimeSpan.FromSeconds(60));

            return channelJoined;

        }

        public String GetPrompt()
        {
            return Environment.NewLine + Environment.NewLine + Commands.GetStatus() + Environment.NewLine
                + $"[{(BotInfo.IsModerator ? "Mod" : "Normal" )} {BotInfo.ChannelName}]> ";
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
            String[] arguments = command.Split(" ");

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
