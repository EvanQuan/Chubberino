using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Models;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Shops
{
    public class Shop : AbstractCommandStrategy, IShop
    {
        public Shop(ApplicationContext context, IMessageSpooler spooler) : base(context, spooler)
        {
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

            Spooler.SpoolMessage($"{message.DisplayName} Cheese Shop (Current cheese: {player.Points})" +
                $" | Buy with \"!cheese buy <item>\"" +
                $" | Get details with \"!cheese help <item>\"" +
                $" | Storage (current: {player.MaximumPointStorage}) +100 @ {storageCost} cheese" +
                $" | Population (current: {player.PopulationCount}) +5 (p)opulation @ {populationCost} cheese (Current: {player.PopulationCount})" +
                $" | +1 (w)orker @ {workerCost} cheese (Current: {player.WorkerCount}) |");
        }

        public void BuyItem(ChatMessage message)
        {
            // Cut out "!cheese buy" start.
            var arguments = message.Message[11..];

            if (String.IsNullOrWhiteSpace(arguments))
            {
                Spooler.SpoolMessage($"{message.DisplayName} Please enter an item to buy. Type \"!cheese shop\" to see the items available for purchase.");
                return;
            }

            var itemToBuy = arguments[1..].ToLower();

            var player = GetPlayer(message);

            var (storageCost, populationCost, workerCost) = GetCosts(player);

            switch (itemToBuy[0])
            {
                case 's':
                    if (player.Points >= storageCost)
                    {
                        player.MaximumPointStorage += 100;
                        player.Points -= storageCost;
                        Context.SaveChanges();
                        Spooler.SpoolMessage($"{message.DisplayName} You bought 100 storage space for {storageCost} cheese. (Current: {player.MaximumPointStorage})");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{message.DisplayName} You need {storageCost} cheese to buy 100 storage. (Current: {player.MaximumPointStorage})");
                    }
                    break;
                case 'p':
                    if (player.Points >= populationCost)
                    {
                        player.PopulationCount += 5;
                        player.Points -= populationCost;
                        Context.SaveChanges();
                        Spooler.SpoolMessage($"{message.DisplayName} You bought 5 population slots for {populationCost} cheese. (Current: {player.MaximumPointStorage})");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{message.DisplayName} You need {populationCost} cheese to buy 5 population slots. (Current: {player.MaximumPointStorage})");
                    }
                    break;
                case 'w':
                    if (player.Points >= workerCost)
                    {
                        player.WorkerCount += 1;
                        player.Points -= workerCost;
                        Context.SaveChanges();
                        Spooler.SpoolMessage($"{message.DisplayName} You bought 1 worker for {workerCost} cheese. (Current: {player.MaximumPointStorage})");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{message.DisplayName} You need {workerCost} cheese to buy 1 worker. (Current: {player.MaximumPointStorage})");
                    }
                    break;
                default:
                    Spooler.SpoolMessage($"{message.DisplayName} Invalid item \"{itemToBuy}\" to buy. Type \"!cheese shop\" to see the items available for purchase.");
                    break;
            }
        }

        public void HelpItem(ChatMessage message)
        {
            // Cut out "!cheese help" start.
            var arguments = message.Message[12..];

            if (String.IsNullOrWhiteSpace(arguments))
            {
                Spooler.SpoolMessage($"{message.DisplayName} Please enter an item name. Type \"!cheese shop\" to see the items available for purchase.");
                return;
            }

            var itemToBuy = arguments[1..].ToLower();

            switch (itemToBuy[0])
            {
                case 's':
                    Spooler.SpoolMessage($"{message.DisplayName} Storage increases the maximum amount of cheese you can have.");
                    break;
                case 'p':
                    Spooler.SpoolMessage($"{message.DisplayName} Population increases the maximum number of workers you can have.");
                    break;
                case 'w':
                    Spooler.SpoolMessage($"{message.DisplayName} Workers increase the amount of cheese you get every time you gain cheese with \"!cheese\".");
                    break;
                default:
                    Spooler.SpoolMessage($"{message.DisplayName} Invalid item \"{itemToBuy}\" name. Type \"!cheese shop\" to see the items available for purchase.");
                    break;
            }
        }
    }
}
