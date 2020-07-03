using MouseBot.Implementation;
using MouseBot.Implementation.Abstractions;
using MouseBot.Implementation.Commands;
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

        private HashSet<String> RelevantUsersToJoin { get; } = new HashSet<String>
        {
            "whydew"
        };

        private IMessageSpooler Spooler { get; set; }

        public String ChannelName { get; set; }

        public Bot()
        {
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 15,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            TwitchClient = new TwitchClient(customClient);

            TwitchClient.Initialize(Credentials, TwitchInfo.InitialChannelName);

            TwitchClient.OnMessageReceived += Client_OnMessageReceived;
            TwitchClient.OnConnected += Client_OnConnected;
            TwitchClient.OnConnectionError += Client_OnConnectionError;
            TwitchClient.OnUserTimedout += Client_OnUserTimedout;
            TwitchClient.OnUserJoined += Client_OnUserJoined;

            Spooler = new MessageSpooler(TwitchClient);
            Commands = new CommandRepository(TwitchClient, Spooler);
        }

        public void Start()
        {
            Console.WriteLine("Connecting to " + TwitchInfo.InitialChannelName);
            TwitchClient.Connect();

            Spooler.Start();
        }

        private void Client_OnUserJoined(Object sender, OnUserJoinedArgs e)
        {
            if (RelevantUsersToJoin.Contains(e.Username))
            {
                TwitchClient.SendMessage(e.Channel, $"yyjYou @{e.Username}");
            }
        }

        private void Client_OnUserTimedout(Object sender, OnUserTimedoutArgs e)
        {
            TwitchClient.SendMessage(e.UserTimeout.Channel, $"WideHardo FREE MY MAN {e.UserTimeout.Username.ToUpper()}");
        }

        private void Client_OnConnectionError(Object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"Error!! {e.Error}");
        }

        private void Client_OnConnected(Object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");

            ChannelName = e.AutoJoinChannel;
            Spooler.SetChannel(ChannelName);

            ShouldContinue = true;
        }

        private void Client_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Username == "Th3Gazette")
            {
                TwitchClient.SendMessage(e.ChatMessage.Channel, $"@{e.ChatMessage.Username} PogO");
            }

            if (e.ChatMessage.Message == "PogO")
            {
                TwitchClient.SendMessage(e.ChatMessage.Channel, $"@{e.ChatMessage.Username} PogO");
            }
        }

        public void ReadCommand(String command)
        {
            String[] arguments = command.Split(" ");

            if (arguments.Length == 0) { return; }

            switch (arguments[0].ToLower())
            {
                case "quit":
                    ShouldContinue = false;
                    break;

                case "box":
                    Int32 count = arguments.Length >= 2
                        ? Int32.Parse(arguments[1])
                        : 1;
                    String surroundEmote = arguments.Length >= 3
                        ? arguments[2]
                        : "yyjW";

                    SpoolJimboBox(surroundEmote, count);
                    break;

                default:
                    if (arguments.Length < 1) { return; }
                    Commands.Execute(arguments[0], arguments.Skip(1).ToArray());
                    break;
            }
        }

        private void SetRepeatMessage(String repeatMessage)
        {
            Spooler.RepeatMessage = repeatMessage;
        }

        private void SpoolJimboBox(String surroundingEmote, Int32 count)
        {
            for (Int32 i = 0; i < count; i++)
            {
                Spooler.SpoolMessage($"{surroundingEmote} {surroundingEmote} {surroundingEmote} {surroundingEmote}");
                Spooler.SpoolMessage($"{surroundingEmote} yyj1 yyj2 {surroundingEmote}");
                Spooler.SpoolMessage($"{surroundingEmote} yyj3 yyj4 {surroundingEmote}");
                Spooler.SpoolMessage($"{surroundingEmote} {surroundingEmote} {surroundingEmote} {surroundingEmote}");
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
