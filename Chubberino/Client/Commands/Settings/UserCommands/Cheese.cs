using Chubberino.Modules.CheeseGame.Heists;
using Chubberino.Modules.CheeseGame.Helping;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Quests;
using Chubberino.Modules.CheeseGame.Ranks;
using Chubberino.Modules.CheeseGame.Shops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public sealed class Cheese : UserCommand
    {
        public Cheese(
            ITwitchClientManager client,
            IConsole console,
            IHelpManager help,
            IPointManager pointManager,
            IShop shop,
            IRankManager rankManager,
            IQuestManager questManager,
            IHeistManager heistManager)
            : base(client, console)
        {
            Help = help;
            PointManager = pointManager;
            Shop = shop;
            RankManager = rankManager;
            QuestManager = questManager;
            HeistManager = heistManager;
            Enable = twitchClient => twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            Disable = twitchClient => twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;

            IsEnabled = TwitchClientManager.IsBot;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (!arguments.Any()) { return; }

            switch (arguments.First().ToLower())
            {
                case "g":
                case "give":
                    if (arguments.Count() < 3) { return; }

                    String name = arguments.Skip(1).FirstOrDefault();

                    Int32 points = Int32.TryParse(arguments.Skip(2).FirstOrDefault(), out points) ? points : 0;

                    PointManager.AddPoints(TwitchClientManager.PrimaryChannelName, name, points);
                    break;
            }
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
                    Help.GetHelp(e.ChatMessage);
                    break;
                case "r":
                case "rank":
                    RankManager.ShowRank(e.ChatMessage);
                    break;
                case "rankup":
                    RankManager.RankUp(e.ChatMessage);
                    break;
                case "q":
                case "quest":
                case "quests":
                    QuestManager.TryStartQuest(e.ChatMessage);
                    break;
                case "heist":
                    // Run in a separate thread as it involves sleeping to wait for joiners.
                    Task.Run(() => HeistManager.InitiateHeist(e.ChatMessage));
                    break;
                default:
                    PointManager.AddPoints(e.ChatMessage);
                    break;
            }
        }

        public IHelpManager Help { get; }
        public IPointManager PointManager { get; }
        public IShop Shop { get; }
        public IRankManager RankManager { get; }
        public IQuestManager QuestManager { get; }
        public IHeistManager HeistManager { get; }
    }
}
