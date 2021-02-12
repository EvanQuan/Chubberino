using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Utility;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Shops
{
    public class Shop : AbstractCommandStrategy, IShop
    {
        public ICheeseRepository CheeseRepository { get; }

        public Shop(ApplicationContext context, IMessageSpooler spooler, ICheeseRepository cheeseRepository) : base(context, spooler)
        {
            CheeseRepository = cheeseRepository;
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

            Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Cheese Shop" +
                $" | Buy with \"!cheese buy <item>\"" +
                $" | Get details with \"!cheese help <item>\"" +
                $" | Recipe [{recipePrompt}" +
                $" | Storage [+100] for {storageCost} cheese" +
                $" | Population [+5] for {populationCost} cheese" +
                $" | Worker [+1] for {workerCost} cheese");
        }

        public void BuyItem(ChatMessage message)
        {
            // Cut out "!cheese buy" start.
            var arguments = message.Message[11..];

            var player = GetPlayer(message);

            if (String.IsNullOrWhiteSpace(arguments))
            {
                Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Please enter an item to buy. Type \"!cheese shop\" to see the items available for purchase.");
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
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You bought 100 storage space. (-{storageCost} cheese)");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You need {storageCost} cheese to buy 100 storage.");
                    }
                    break;
                case 'p':
                    if (player.Points >= populationCost)
                    {
                        player.PopulationCount += 5;
                        player.Points -= populationCost;
                        Context.SaveChanges();
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You bought 5 population slots. (-{populationCost} cheese)");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You need {populationCost - player.Points} more cheese to buy 5 populationslots.");
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
                            Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You bought 1 worker. (-{workerCost} cheese)");
                        }
                        else
                        {
                            Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You do not have enough population slots for another worker. Consider buying more population slots.");
                        }
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You need {workerCost - player.Points} more cheese to buy 1 worker.");
                    }
                    break;
                case 'r':
                    var nextCheeseToUnlock = CheeseRepository.GetNextCheeseToUnlock(player);
                    if (nextCheeseToUnlock == null || nextCheeseToUnlock.RankToUnlock > player.Rank)
                    {
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} There is no recipe for sale right now.");
                    }
                    else if (player.Points >= nextCheeseToUnlock.CostToUnlock)
                    {
                        player.CheeseUnlocked++;
                        player.Points -= nextCheeseToUnlock.CostToUnlock; ;
                        Context.SaveChanges();
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You unlocked the {nextCheeseToUnlock.Name} recipe. (-{nextCheeseToUnlock.CostToUnlock} cheese)");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You need {nextCheeseToUnlock.PointValue - player.Points} more cheese to unlock the {nextCheeseToUnlock.Name} recipe.");
                    }
                    break;
                default:
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Invalid item \"{itemToBuy}\" to buy. Type \"!cheese shop\" to see the items available for purchase.");
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
                Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Please enter an item name. Type \"!cheese shop\" to see the items available for purchase.");
                return;
            }

            var itemToBuy = arguments[1..].ToLower();

            switch (itemToBuy)
            {
                case "s":
                case "storage":
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Storage increases the maximum amount of cheese you can have.");
                    break;
                case "p":
                case "population":
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Population increases the maximum number of workers you can have.");
                    break;
                case "w":
                case "worker":
                case "workers":
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Workers increase the amount of cheese you get every time you gain cheese with \"!cheese\".");
                    break;
                case "recipe":
                case "recipes":
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Recipes allow you to create new kinds of cheese with \"!cheese\".");
                    break;
                case "r":
                case "rank":
                case "ranks":
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Ranks unlock new items to buy at the shop. Eventually ranking will give you prestige, reseting your rank and everything you have to restart the climb. For every prestige you gain, you get a permanent {(Int32)(Constants.PrestigeBonus * 100)}% boost to your cheese gains, which can stack.");
                    break;
                default:
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Invalid item \"{itemToBuy}\" name. Type \"!cheese shop\" to see the items available for purchase.");
                    break;
            }
        }
    }
}
