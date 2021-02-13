using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Upgrades;
using Chubberino.Utility;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Shops
{
    public class Shop : AbstractCommandStrategy, IShop
    {
        public ICheeseRepository CheeseRepository { get; }
        public IUpgradeManager UpgradeManager { get; }

        public Shop(
            ApplicationContext context,
            IMessageSpooler spooler,
            ICheeseRepository cheeseRepository,
            Random random,
            IEmoteManager emoteManager,
            IUpgradeManager upgradeManager)
            : base(context, spooler, random, emoteManager)
        {
            CheeseRepository = cheeseRepository;
            UpgradeManager = upgradeManager;
        }

        private (Int32 Storage, Int32 Population, Int32 Worker) GetCosts(Player player)
        {
            var storageCost = 25 + player.MaximumPointStorage / 2;
            var populationCost = (Int32)(20 + Math.Pow(player.PopulationCount, 2));
            var workerCost = (Int32)(100 + 5 * Math.Pow(player.WorkerCount, 2));

            return (storageCost, populationCost, workerCost);
        }

        public void ListInventory(ChatMessage message)
        {
            var player = GetPlayer(message);

            var (storageCost, populationCost, workerCost) = GetCosts(player);

            var nextCheeseToUnlock = CheeseRepository.GetNextCheeseToUnlock(player);

            String recipePrompt;

            if (nextCheeseToUnlock == null)
            {
                recipePrompt = "OUT OF ORDER]";
            }
            else if (nextCheeseToUnlock.RankToUnlock > player.Rank)
            {
                recipePrompt = $"{nextCheeseToUnlock.Name} (+{nextCheeseToUnlock.PointValue})] unlocked at {player.Rank.Next()} rank"; 
            }
            else
            {
                recipePrompt = $"{nextCheeseToUnlock.Name} (+{nextCheeseToUnlock.PointValue})] for {nextCheeseToUnlock.CostToUnlock} cheese"; 
            }

            var upgrade = UpgradeManager.GetNextUpgradeToUnlock(player);

            String upgradePrompt;

            if (upgrade == null)
            {
                upgradePrompt = "OUT OF ORDER]";
            }
            else if (upgrade.RankToUnlock > player.Rank)
            {
                upgradePrompt = $"{upgrade.Description}] unlocked at {upgrade.RankToUnlock} rank";
            }
            else
            {
                upgradePrompt = $"{upgrade.Description}] for {upgrade.Price} cheese";
            }

            Spooler.SpoolMessage($"{player.GetDisplayName()} Cheese Shop" +
                $" | Buy with \"!cheese buy <item>\"" +
                $" | Get details with \"!cheese help <item>\"" +
                $" | Recipe [{recipePrompt}" +
                $" | Storage [+100] for {storageCost} cheese" +
                $" | Population [+5] for {populationCost} cheese" +
                $" | Worker [+1] for {workerCost} cheese" +
                $" | Upgrade worker [{upgradePrompt}");
        }

        public void BuyItem(ChatMessage message)
        {
            // Cut out "!cheese buy" start.
            var arguments = message.Message[11..];

            var player = GetPlayer(message);

            if (String.IsNullOrWhiteSpace(arguments))
            {
                Spooler.SpoolMessage($"{player.GetDisplayName()} Please enter an item to buy. Type \"!cheese shop\" to see the items available for purchase.");
                return;
            }

            var itemToBuy = arguments[1..].ToLower();


            var (storageCost, populationCost, workerCost) = GetCosts(player);

            switch (itemToBuy[0])
            {
                case 's':
                    if (player.Points >= storageCost)
                    {
                        player.MaximumPointStorage += 100;
                        player.Points -= storageCost;
                        Context.SaveChanges();
                        Spooler.SpoolMessage($"{player.GetDisplayName()} You bought 100 storage space. (-{storageCost} cheese)");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{player.GetDisplayName()} You need {storageCost} cheese to buy 100 storage.");
                    }
                    break;
                case 'p':
                    if (player.Points >= populationCost)
                    {
                        player.PopulationCount += 5;
                        player.Points -= populationCost;
                        Context.SaveChanges();
                        Spooler.SpoolMessage($"{player.GetDisplayName()} You bought 5 population slots. (-{populationCost} cheese)");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{player.GetDisplayName()} You need {populationCost - player.Points} more cheese to buy 5 populationslots.");
                    }
                    break;
                case 'w':
                    if (player.Points >= workerCost)
                    {
                        if (player.WorkerCount < player.PopulationCount)
                        {
                            player.WorkerCount += 1;
                            player.Points -= workerCost;
                            Context.SaveChanges();
                            Spooler.SpoolMessage($"{player.GetDisplayName()} You bought 1 worker. (-{workerCost} cheese)");
                        }
                        else
                        {
                            Spooler.SpoolMessage($"{player.GetDisplayName()} You do not have enough population slots for another worker. Consider buying more population slots.");
                        }
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{player.GetDisplayName()} You need {workerCost - player.Points} more cheese to buy 1 worker.");
                    }
                    break;
                case 'r':
                    var nextCheeseToUnlock = CheeseRepository.GetNextCheeseToUnlock(player);
                    if (nextCheeseToUnlock == null)
                    {
                        Spooler.SpoolMessage($"{player.GetDisplayName()} There is no recipe for sale right now.");
                    }
                    else if (nextCheeseToUnlock.RankToUnlock > player.Rank)
                    {
                        Spooler.SpoolMessage($"{player.GetDisplayName()} You must rankup to {nextCheeseToUnlock.RankToUnlock} rank before you can buy the {nextCheeseToUnlock.Name} recipe.");
                    }
                    else if (player.Points >= nextCheeseToUnlock.CostToUnlock)
                    {
                        player.CheeseUnlocked++;
                        player.Points -= nextCheeseToUnlock.CostToUnlock;
                        Context.SaveChanges();
                        Spooler.SpoolMessage($"{player.GetDisplayName()} You bought the {nextCheeseToUnlock.Name} recipe. (-{nextCheeseToUnlock.CostToUnlock} cheese)");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{player.GetDisplayName()} You need {nextCheeseToUnlock.CostToUnlock - player.Points} more cheese to buy the {nextCheeseToUnlock.Name} recipe.");
                    }
                    break;
                case 'u':
                    var upgrade = UpgradeManager.GetNextUpgradeToUnlock(player);
                    if (upgrade == null)
                    {
                        Spooler.SpoolMessage($"{player.GetDisplayName()} There is no upgrade for sale right now.");
                    }
                    else if (upgrade.RankToUnlock > player.Rank)
                    {
                        Spooler.SpoolMessage($"{player.GetDisplayName()} You must rankup to {upgrade.RankToUnlock} rank before you can buy the {upgrade.Description} upgrade.");
                    }
                    else if (player.Points >= upgrade.Price)
                    {
                        upgrade.UpdatePlayer(player);
                        player.Points -= upgrade.Price;
                        Context.SaveChanges();
                        Spooler.SpoolMessage($"{player.GetDisplayName()} You bought the {upgrade.Description} upgrade. (-{upgrade.Price} cheese)");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{player.GetDisplayName()} You need {upgrade.Price - player.Points} more cheese to buy the {upgrade.Description} upgrade.");
                    }
                    break;
                default:
                    Spooler.SpoolMessage($"{player.GetDisplayName()} Invalid item \"{itemToBuy}\" to buy. Type \"!cheese shop\" to see the items available for purchase.");
                    break;
            }
        }

        public void HelpItem(ChatMessage message)
        {
            // Cut out "!cheese help" start.
            var arguments = message.Message[12..];

            var player = GetPlayer(message);

            if (String.IsNullOrWhiteSpace(arguments))
            {
                Spooler.SpoolMessage($"{player.GetDisplayName()} Commands: !cheese <command> where command is " +
                    $"| shop - look at what is available to buy with cheese " +
                    $"| buy <item> - buy an item at the shop " +
                    $"| help <item> - get information about an item in the shop " +
                    $"| quest - potentially get a big reward, or risk failure. The more workers you have, the greater chance of success. " +
                    $"| rank - show information about your rank " +
                    $"| rankup - Spend cheese to unlock new items to buy at the shop. Eventually prestige back to the start to climb again but with a permanent boost to your cheese gains.");
                return;
            }

            var itemToBuy = arguments[1..].ToLower();

            switch (itemToBuy)
            {
                case "s":
                case "storage":
                    Spooler.SpoolMessage($"{player.GetDisplayName()} Storage increases the maximum amount of cheese you can have.");
                    break;
                case "p":
                case "population":
                    Spooler.SpoolMessage($"{player.GetDisplayName()} Population increases the maximum number of workers you can have.");
                    break;
                case "w":
                case "worker":
                case "workers":
                    Spooler.SpoolMessage($"{player.GetDisplayName()} Workers increase the amount of cheese you get every time you gain cheese with \"!cheese\".");
                    break;
                case "q":
                case "quest":
                case "quests":
                    Spooler.SpoolMessage($"{player.GetDisplayName()} Go on a random quest to get rewards or risk punishment. The chance of success scales with how many workers you have.");
                    break;
                case "recipe":
                case "recipes":
                    Spooler.SpoolMessage($"{player.GetDisplayName()} Recipes allow you to create new kinds of cheese with \"!cheese\".");
                    break;
                case "r":
                case "rank":
                case "ranks":
                    Spooler.SpoolMessage($"{player.GetDisplayName()} Ranks unlock new items to buy at the shop. Eventually ranking will give you prestige, reseting your rank and everything you have to restart the climb. For every prestige you gain, you get a permanent {(Int32)(Constants.PrestigeBonus * 100)}% boost to your cheese gains, which can stack.");
                    break;
                default:
                    Spooler.SpoolMessage($"{player.GetDisplayName()} Invalid item \"{itemToBuy}\" name. Type \"!cheese shop\" to see the items available for purchase.");
                    break;
            }
        }
    }
}
