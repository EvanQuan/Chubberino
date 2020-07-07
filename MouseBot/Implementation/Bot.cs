using MouseBot.Implementation;
using MouseBot.Implementation.Abstractions;
using MouseBot.Implementation.Commands;
using MouseBot.Implementation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace MouseBot
{
    internal class Bot : IDisposable
    {

        public Boolean ShouldContinue { get; private set; }

        private ITwitchClient TwitchClient { get; set; }

        private ConnectionCredentials Credentials { get; } = new ConnectionCredentials(TwitchInfo.BotUsername, TwitchInfo.BotToken);

        private CommandRepository Commands { get; }

        private IMessageSpooler Spooler { get; set; }

        public Bot()
        {
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 1,
                ThrottlingPeriod = TimeSpan.FromSeconds(1.55)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            TwitchClient = new TwitchClient(customClient);

            TwitchClient.Initialize(Credentials, TwitchInfo.InitialChannelName);

            TwitchClient.OnMessageReceived += Client_OnMessageReceived;
            TwitchClient.OnConnected += Client_OnConnected;
            TwitchClient.OnConnectionError += Client_OnConnectionError;
            TwitchClient.OnUserTimedout += Client_OnUserTimedout;

            Spooler = new MessageSpooler(TwitchClient);
            Commands = new CommandRepository(TwitchClient, Spooler);
        }

        public Boolean Start()
        {
            Console.WriteLine("Connecting to " + TwitchInfo.InitialChannelName);
            TwitchClient.EnsureJoinedToChannel(TwitchInfo.InitialChannelName);

            Spooler.Start();

            Boolean channelJoined = SpinWait.SpinUntil(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                return !String.IsNullOrWhiteSpace(Spooler.ChannelName);
            },
            TimeSpan.FromSeconds(60));


            return channelJoined;

        }

        public String GetPrompt()
        {
            return Environment.NewLine + Environment.NewLine + Commands.GetStatus() + Environment.NewLine
                + $"[{Spooler.ChannelName}]> ";
        }

        private void Client_OnUserTimedout(Object sender, OnUserTimedoutArgs e)
        {
        }

        private void Client_OnConnectionError(Object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"Error!! {e.Error}");
        }

        private void Client_OnConnected(Object sender, OnConnectedArgs e)
        {
            ShouldContinue = true;
        }

        private void Client_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Username.Equals("Th3Gazette", StringComparison.OrdinalIgnoreCase))
            {
                TwitchClient.SendMessage(e.ChatMessage.Channel, $"@{e.ChatMessage.Username} PogO");
            }
        }

        public void ReadCommand(String command)
        {
            String[] arguments = command.Split(" ");

            if (arguments.Length == 0) { return; }

            String commandName = arguments[0].ToLower();

            switch (commandName)
            {
                case "quit":
                    ShouldContinue = false;
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
