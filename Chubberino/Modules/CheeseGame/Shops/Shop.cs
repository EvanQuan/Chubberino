using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Items;
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
        public IItemManager ItemManager { get; }

        public Shop(
            ApplicationContext context,
            IMessageSpooler spooler,
            ICheeseRepository cheeseRepository,
            Random random,
            IEmoteManager emoteManager,
            IUpgradeManager upgradeManager,
            IItemManager itemManager)
            : base(context, spooler, random, emoteManager)
        {
            CheeseRepository = cheeseRepository;
            UpgradeManager = upgradeManager;
            ItemManager = itemManager;
        }

        public void ListInventory(ChatMessage message)
        {
            var player = GetPlayer(message);

            var prices = ItemManager.GetPrices(player);

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

            Spooler.SpoolMessage(message.Channel,
                $"{player.GetDisplayName()}" +
                $" | Recipe [{recipePrompt}" +
                $" | Storage [+100] for {prices.Storage} cheese" +
                $" | Population [+5] for {prices.Population} cheese" +
                $" | Worker [+1] for {prices.Worker} cheese" +
                $" | Upgrade [{upgradePrompt}" + 
                $" | Mousetrap [+1] for {prices.MouseTrap} " +
                "|");
        }

        public void BuyItem(ChatMessage message)
        {
            // Cut out "!cheese buy" start.
            var arguments = message.Message[11..];

            var player = GetPlayer(message);

            if (String.IsNullOrWhiteSpace(arguments))
            {
                Spooler.SpoolMessage(message.Channel, $"{player.GetDisplayName()} Please enter an item to buy. Type \"!cheese shop\" to see the items available for purchase.");
                return;
            }

            var itemToBuy = arguments[1..].ToLower();


            var prices = ItemManager.GetPrices(player);

            String outputMessage = player.GetDisplayName() + " ";

            switch (itemToBuy[0])
            {
                case 's':
                    if (player.Points >= prices.Storage)
                    {
                        player.MaximumPointStorage += 100;
                        player.Points -= prices.Storage;
                        Context.SaveChanges();
                        outputMessage += $"You bought 100 storage space. (-{prices.Storage} cheese)";
                    }
                    else
                    {
                        outputMessage += $"You need {prices.Storage} cheese to buy 100 storage.";
                    }
                    break;
                case 'p':
                    if (player.Points >= prices.Population)
                    {
                        player.PopulationCount += 5;
                        player.Points -= prices.Population;
                        Context.SaveChanges();
                        outputMessage += $"You bought 5 population slots. (-{prices.Population} cheese)";
                    }
                    else
                    {
                        outputMessage += $"You need {prices.Population - player.Points} more cheese to buy 5 population slots.";
                    }
                    break;
                case 'w':
                    if (player.Points >= prices.Worker)
                    {
                        if (player.WorkerCount < player.PopulationCount)
                        {
                            player.WorkerCount += 1;
                            player.Points -= prices.Worker;
                            Context.SaveChanges();
                            outputMessage += $"You bought 1 worker. (-{prices.Worker} cheese)";
                        }
                        else
                        {
                            outputMessage += $"You do not have enough population slots for another worker. Consider buying more population slots.";
                        }
                    }
                    else
                    {
                        outputMessage += $"You need {prices.Worker - player.Points} more cheese to buy 1 worker.";
                    }
                    break;
                case 'r':
                    var nextCheeseToUnlock = CheeseRepository.GetNextCheeseToUnlock(player);
                    if (nextCheeseToUnlock == null)
                    {
                        outputMessage += $"There is no recipe for sale right now.";
                    }
                    else if (nextCheeseToUnlock.RankToUnlock > player.Rank)
                    {
                        outputMessage += $"You must rankup to {nextCheeseToUnlock.RankToUnlock} rank before you can buy the {nextCheeseToUnlock.Name} recipe.";
                    }
                    else if (player.Points >= nextCheeseToUnlock.CostToUnlock)
                    {
                        player.CheeseUnlocked++;
                        player.Points -= nextCheeseToUnlock.CostToUnlock;
                        Context.SaveChanges();
                        outputMessage += $"You bought the {nextCheeseToUnlock.Name} recipe. (-{nextCheeseToUnlock.CostToUnlock} cheese)";
                    }
                    else
                    {
                        outputMessage += $"You need {nextCheeseToUnlock.CostToUnlock - player.Points} more cheese to buy the {nextCheeseToUnlock.Name} recipe.";
                    }
                    break;
                case 'u':
                    var upgrade = UpgradeManager.GetNextUpgradeToUnlock(player);
                    if (upgrade == null)
                    {
                        outputMessage += $"There is no upgrade for sale right now.";
                    }
                    else if (upgrade.RankToUnlock > player.Rank)
                    {
                        outputMessage += $"You must rankup to {upgrade.RankToUnlock} rank before you can buy the {upgrade.Description} upgrade.";
                    }
                    else if (player.Points >= upgrade.Price)
                    {
                        upgrade.UpdatePlayer(player);
                        player.Points -= upgrade.Price;
                        Context.SaveChanges();
                        outputMessage += $"You bought the {upgrade.Description} upgrade. (-{upgrade.Price} cheese)";
                    }
                    else
                    {
                        outputMessage += $"You need {upgrade.Price - player.Points} more cheese to buy the {upgrade.Description} upgrade.";
                    }
                    break;
                case 'm':
                    if (player.Points >= prices.MouseTrap)
                    {
                        player.MouseTrapCount++;
                        player.Points -= prices.MouseTrap;
                        Context.SaveChanges();
                        outputMessage += $"You bought 1 mousetrap. (-{prices.MouseTrap} cheese)";
                    }
                    else
                    {
                        outputMessage += $"You need {prices.MouseTrap - player.Points} more cheese to buy 1 mousetrap.";
                    }
                    break;
                default:
                    outputMessage += $"Invalid item \"{itemToBuy}\" to buy. Type \"!cheese shop\" to see the items available for purchase.";
                    break;
            }

            Spooler.SpoolMessage(message.Channel, outputMessage);
        }

        public void HelpItem(ChatMessage message)
        {
            // Cut out "!cheese help" start.
            var arguments = message.Message.StartsWith("!cheese help")
                ? message.Message[12..]
                : message.Message[9..]; // !cheese h

            var player = GetPlayer(message);

            if (String.IsNullOrWhiteSpace(arguments))
            {
                Spooler.SpoolMessage(message.Channel,
                    $"{player.Name} Commands: !cheese <command> where command is " +
                    $"| shop - look at what is available to buy with cheese " +
                    $"| buy <item> - buy an item at the shop " +
                    $"| help <item> - get information about an item in the shop " +
                    $"| quest - potentially get a big reward, or risk failure. The more workers you have, the greater chance of success. " +
                    $"| rank - show information about your rank " +
                    $"| rankup - Spend cheese to unlock new items to buy at the shop. Eventually prestige back to the start to climb again but with a permanent boost to your cheese gains.");
                return;
            }

            var itemToBuy = arguments[1..].ToLower();

            String outputMessage = player.GetDisplayName() + " ";
            switch (itemToBuy)
            {
                case "s":
                case "storage":
                    outputMessage += $"Storage increases the maximum amount of cheese you can have.";
                    break;
                case "p":
                case "population":
                    outputMessage += $"Population increases the maximum number of workers you can have.";
                    break;
                case "w":
                case "worker":
                case "workers":
                    outputMessage += $"Workers increase the amount of cheese you get every time you gain cheese with \"!cheese\".";
                    break;
                case "q":
                case "quest":
                case "quests":
                    outputMessage += $"Go on a random quest to get rewards or risk punishment. The chance of success scales with how many workers you have.";
                    break;
                case "recipe":
                case "recipes":
                    outputMessage += $"Recipes allow you to create new kinds of cheese with \"!cheese\".";
                    break;
                case "r":
                case "rank":
                case "ranks":
                    outputMessage += $"Ranks unlock new items to buy at the shop. Eventually ranking will give you prestige, reseting your rank and everything you have to restart the climb. For every prestige you gain, you get a permanent {(Int32)(Constants.PrestigeBonus * 100)}% boost to your cheese gains, which can stack.";
                    break;
                case "u":
                case "upgrade":
                case "upgrades":
                    outputMessage += $"Upgrades provide a permanent bonus to your cheese factory until you prestige.";
                    break;
                case "m":
                case "mouse":
                case "mousetrap":
                case "mousetraps":
                    outputMessage += $"Mousetraps kills giant rats that infest your cheese factory.";
                    break;
                default:
                    outputMessage += $"Invalid item \"{itemToBuy}\" name. Type \"!cheese shop\" to see the items available for purchase.";
                    break;
            }

            Spooler.SpoolMessage(message.Channel, outputMessage);
        }
    }
}
