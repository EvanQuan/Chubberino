using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Modules.CheeseGame.Shops;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public sealed class Cheese : UserCommand
    {
        public Cheese(IExtendedClient client, TextWriter console, IPointManager pointManager, IShop shop, IRankManager rankManager) : base(client, console)
        {
            PointManager = pointManager;
            Shop = shop;
            RankManager = rankManager;
            Enable = twitchClient => twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            Disable = twitchClient => twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!TryValidateCommand(e, out IEnumerable<String> words)) { return; }

            if (!words.Any())
            {
                PointManager.AddPoints(e.ChatMessage);
                return;
            }

            var cheeseCommand = words.First().ToLower();
            switch (cheeseCommand)
            {
                case "s":
                case "shop":
                    Shop.ListInventory(e.ChatMessage);
                    break;
                case "b":
                case "buy":
                    Shop.BuyItem(e.ChatMessage);
                    break;
                case "h":
                case "help":
                    Shop.HelpItem(e.ChatMessage);
                    break;
                case "r":
                case "rank":
                case "rankup":
                    RankManager.RankUp(e.ChatMessage);
                    break;
                default:
                    TwitchClient.SpoolMessage($"Invalid parameter \"{cheeseCommand}\"");
                    break;
            }
        }

        public IPointManager PointManager { get; }
        public IShop Shop { get; }
        public IRankManager RankManager { get; }
    }
}
