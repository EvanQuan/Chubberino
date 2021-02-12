using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Points;
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

        public override void Refresh(IExtendedClient twitchClient)
        {
            base.Refresh(twitchClient);

            PointManager.Spooler = TwitchClient;
            Shop.Spooler = TwitchClient;
            RankManager.Spooler = TwitchClient;
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
                    RankManager.ShowRank(e.ChatMessage);
                    break;
                case "rankup":
                    RankManager.RankUp(e.ChatMessage);
                    break;
                case "c":
                case "command":
                case "commands":
                    TwitchClient.SpoolMessage($"{e.ChatMessage.DisplayName} Commands: !cheese <command> where command is | shop - look at what is available to buy with cheese | buy <item> - buy an item at the shop | help <item> - get information about an item in the shop | rank - show information about your rank | rankup - Spend cheese to unlock new items to buy at the shop. Eventually prestige back to the start to climb again but with a permanent boost to your cheese gains.");
                    break;
                default:
                    PointManager.AddPoints(e.ChatMessage);
                    break;
            }
        }

        public IPointManager PointManager { get; }
        public IShop Shop { get; }
        public IRankManager RankManager { get; }
    }
}
