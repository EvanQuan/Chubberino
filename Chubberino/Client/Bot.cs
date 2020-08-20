using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands;
using Chubberino.Client.Extensions;
using System;
using System.Linq;
using System.Threading;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace Chubberino.Client
{
    internal class Bot : IDisposable
    {
        public static Bot Instance { get; } = new Bot();

        private TimeSpan CurrentThrottlingPeriod { get; set; }

        public BotState State { get; private set; }

        private IExtendedClient TwitchClient { get; set; }

        private ConnectionCredentials Credentials { get; } = new ConnectionCredentials(TwitchInfo.BotUsername, TwitchInfo.BotToken);

        private CommandRepository Commands { get; set; }

        private void CreateClient()
        {
            InitializeTwitchClientAndSpooler(BotInfo.Instance.RegularThrottlingPeriod);
            Commands = new CommandRepository(TwitchClient);
        }

        private Bot()
        {
            CreateClient();
        }

        private void InitializeTwitchClientAndSpooler(TimeSpan throttlingPeriod)
        {
            CurrentThrottlingPeriod = throttlingPeriod;

            // Determine whether to throttle at regular or mod speed.
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 1,
                ThrottlingPeriod = throttlingPeriod
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            TwitchClient = new ExtendedClient(customClient);

            TwitchClient.Initialize(Credentials, TwitchInfo.InitialChannelName);

            TwitchClient.OnConnected += Client_OnConnected;
            TwitchClient.OnConnectionError += Client_OnConnectionError;
            TwitchClient.OnUserTimedout += Client_OnUserTimedout;
        }

        public Boolean Start()
        {
            Console.WriteLine("Connecting to " + TwitchInfo.InitialChannelName);
            TwitchClient.EnsureJoinedToChannel(TwitchInfo.InitialChannelName);

            Boolean channelJoined = SpinWait.SpinUntil(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                return !String.IsNullOrWhiteSpace(BotInfo.Instance.ChannelName);
            },
            TimeSpan.FromSeconds(60));

            return channelJoined;

        }

        public String GetPrompt()
        {
            return Environment.NewLine + Environment.NewLine + Commands.GetStatus() + Environment.NewLine
                + $"[{(BotInfo.Instance.IsModerator ? "Mod" : "Normal" )} {BotInfo.Instance.ChannelName}]> ";
        }

        private void Client_OnUserTimedout(Object sender, OnUserTimedoutArgs e)
        {
        }

        private void Client_OnConnectionError(Object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"!! Connection Error!! {e.Error.Message}");
            Refresh(CurrentThrottlingPeriod);
            Console.WriteLine("!! Refreshed");
        }

        public void Refresh(TimeSpan throttlePeriod)
        {
            InitializeTwitchClientAndSpooler(throttlePeriod);
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
