using System.Collections.Generic;
using System.IO;
using Chubberino.Bots.Channel.Modules.CheeseGame.Heists;
using Chubberino.Bots.Channel.Modules.CheeseGame.Helping;
using Chubberino.Bots.Channel.Modules.CheeseGame.Points;
using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Bots.Channel.Modules.CheeseGame.Ranks;
using Chubberino.Bots.Channel.Modules.CheeseGame.Shops;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings.UserCommands;

namespace Chubberino.Bots.Channel.Commands;

public sealed class Cheese : UserCommand
{
    public Cheese(
        ITwitchClientManager client,
        TextWriter writer,
        IHelpManager help,
        IPointManager pointManager,
        IShop shop,
        IRankManager rankManager,
        IQuestManager questManager,
        IHeistManager heistManager)
        : base(client, writer)
    {
        Help = help;
        PointManager = pointManager;
        Shop = shop;
        RankManager = rankManager;
        QuestManager = questManager;
        HeistManager = heistManager;
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

    public override void Invoke(Object sender, OnUserCommandReceivedArgs e)
    {
        if (!e.Words.Any())
        {
            PointManager.AddPoints(e.ChatMessage);
            return;
        }

        var cheeseCommand = e.Words.First().ToLower();
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
                HeistManager.Heist(e.ChatMessage);
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
