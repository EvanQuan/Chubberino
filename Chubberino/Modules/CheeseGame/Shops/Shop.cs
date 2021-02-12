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

            Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Cheese Shop" +
                $" | Buy with \"!cheese buy <item>\"" +
                $" | Get details with \"!cheese help <item>\"" +
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
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You bought 100 storage space for {storageCost} cheese. (Current: {player.MaximumPointStorage})");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You need {storageCost} cheese to buy 100 storage. (Current: {player.MaximumPointStorage})");
                    }
                    break;
                case 'p':
                    if (player.Points >= populationCost)
                    {
                        player.PopulationCount += 5;
                        player.Points -= populationCost;
                        Context.SaveChanges();
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You bought 5 population slots for {populationCost} cheese. (Current: {player.PopulationCount})");
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You do not have enough population slots for another worker. Consider buying more population slots. (Current: {player.PopulationCount})");
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
                            Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You bought 1 worker for {workerCost} cheese. (Current: {player.WorkerCount})");
                        }
                        else
                        {
                            Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You bought 1 worker for {workerCost} cheese. (Current: {player.WorkerCount})");
                        }
                    }
                    else
                    {
                        Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You need {workerCost} cheese to buy 1 worker. (Current: {player.WorkerCount})");
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

            switch (itemToBuy[0])
            {
                case 's':
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Storage increases the maximum amount of cheese you can have.");
                    break;
                case 'p':
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Population increases the maximum number of workers you can have.");
                    break;
                case 'w':
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Workers increase the amount of cheese you get every time you gain cheese with \"!cheese\".");
                    break;
                default:
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Invalid item \"{itemToBuy}\" name. Type \"!cheese shop\" to see the items available for purchase.");
                    break;
            }
        }
    }
}
