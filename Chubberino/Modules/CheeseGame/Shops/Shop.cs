using Chubberino.Client;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Repositories;
using Chubberino.Modules.CheeseGame.Upgrades;
using Chubberino.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Shops
{
    public class Shop : AbstractCommandStrategy, IShop
    {
        public IRepository<CheeseType> CheeseRepository { get; }
        public IRepository<Quests.Quest> QuestRepository { get; }
        public IItemManager ItemManager { get; }

        public IList<IItem> Items { get; }

        public Shop(
            IApplicationContext context,
            ITwitchClientManager client,
            IRepository<CheeseType> cheeseRepository,
            IRepository<Quests.Quest> questRepository,
            Random random,
            IEmoteManager emoteManager,
            IItemManager itemManager)
            : base(context, client, random, emoteManager)
        {
            CheeseRepository = cheeseRepository;
            QuestRepository = questRepository;
            ItemManager = itemManager;
            Items = new List<IItem>();
        }

        public void ListInventory(ChatMessage message)
        {
            Player player = GetPlayer(message);

            StringBuilder inventoryPrompt = new();

            foreach (var item in Items)
            {
                inventoryPrompt
                    .Append(" | ")
                    .Append(item.GetShopPrompt(player));
            }

            TwitchClientManager.SpoolMessageAsMe(message.Channel, player, inventoryPrompt.ToString(), Priority.Low);
        }

        public void BuyItem(ChatMessage message)
        {
            // Cut out "!cheese buy " or "!cheese b" start.
            String arguments = message.Message
                .GetNextWord(out _)
                .GetNextWord(out _);

            Player player = GetPlayer(message);

            if (String.IsNullOrWhiteSpace(arguments))
            {
                TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"Please enter an item to buy with \"!cheese buy <name of item>\". Type \"!cheese shop\" to see the items available for purchase.", Priority.Low);
                return;
            }

            // Cut out space between buy and item
            String remainingArguments = arguments.GetNextWord(out String itemToBuy);

            PriceList prices = ItemManager.GetPrices(player);

            String outputMessage;

            Priority priority = Priority.Low;

            if (Items.TryGetFirst(x => x.Names.Contains(itemToBuy, StringComparer.InvariantCultureIgnoreCase), out var item))
            {
                remainingArguments.GetNextWord(out String quantityString);

                Int32 quantityRequested = Int32.TryParse(quantityString, out Int32 quantityParsed) && quantityParsed > 0
                    ? quantityParsed
                    : new String[] { "a", "all" }.Contains(quantityString, StringComparer.InvariantCultureIgnoreCase)
                        ? Int32.MaxValue
                        : 1;

                var result = item.TryBuy(quantityRequested, player)();

                if (result.IsLeft)
                {
                    var buyResult = result.Left;
                    outputMessage = $"You bought {item.GetSpecificNameForSuccessfulBuy(player, buyResult.QuantityPurchased)}. {EmoteManager.GetRandomPositiveEmote(message.Channel)} (-{buyResult.PointsSpent} cheese)";
                    priority = Priority.Medium;
                }
                else
                {
                    outputMessage = result.Right;
                }
            }
            else
            {
                outputMessage = $"Invalid item \"{itemToBuy}\" to buy. Type \"!cheese shop\" to see the items available for purchase.";
            }

            TwitchClientManager.SpoolMessageAsMe(message.Channel, player, outputMessage, priority);
        }

        public void HelpItem(ChatMessage message)
        {
            // Cut out "!cheese help" start.
            String arguments = message.Message
                .StripStart("!cheese h")
                .StripStart("elp");

            Player player = GetPlayer(message);

            if (String.IsNullOrWhiteSpace(arguments))
            {
                TwitchClientManager.SpoolMessageAsMe(message.Channel, 
                    $"{player.Name} Commands: !cheese <command> where command is " +
                    $"| shop - look at what is available to buy with cheese " +
                    $"| buy <item> - buy an item at the shop " +
                    $"| help <item> - get information about an item in the shop " +
                    $"| quest - potentially get a big reward. The more workers you have, the greater chance of success. " +
                    $"| heist - Gamble some cheese, to potentially get more in return. " +
                    $"| rank - show information about your rank " +
                    $"| rankup - Spend cheese to unlock new items to buy at the shop.",
                    Priority.Low);
                return;
            }

            String itemToBuy = arguments[1..].ToLower();

            String outputMessage = itemToBuy switch
            {
                "s" or "storage" => $"Storage increases the maximum amount of cheese you can have.",
                "p" or "population" => $"Population increases the maximum number of workers you can have.",
                "w" or "worker" or "workers" => $"Workers increase the amount of cheese you get every time you gain cheese with \"!cheese\" and when you go on a quest with \"!cheese quest\". Initially they each give an additive {RankExtensions.BaseWorkerPointPercent * 100}% bonus to cheese gains.",
                "q" or "quest" or "quests" => $"Go on a random quest to get rewards. The chance of success scales with how much gear you have.",
                "recipe" or "recipes" => $"Recipes allow you to create new kinds of cheese with \"!cheese\".",
                "r" or "rank" or "ranks" or "bronze" or "silver" or "gold" or "diamond" or "platinum" or "master" or "grandmaster" or "legend" => $"Ranks unlock new items to buy at the shop. Eventually ranking will give you prestige, reseting your rank and everything you have to restart the climb. For every prestige you gain, you get a permanent {(Int32)(Constants.PrestigeBonus * 100)}% boost to your cheese gains, which can stack.",
                "u" or "upgrade" or "upgrades" => $"Upgrades provide a permanent bonus to your cheese factory until you prestige.",
                "g" or "gear" => $"Gear provides you with a {(Int32)(Constants.QuestGearSuccessPercent * 100)}% quest success chance for each you have.",
                "m" or "mouse" or "mousetrap" or "mousetraps" => $"Mousetraps kills giant mice that infest your cheese factory, allow you to maintain or recover any worker bonuses you have.",
                "c" or "cat" or "cats" => $"[CURRENTLY DO NOTHING] Cats help you fight against the giant evil mouse, Chubshan the Immortal. The more cats you have, the more you will be rewarded when Chubshan is defeated.",
                _ => $"Invalid item \"{itemToBuy}\" name. Type \"!cheese shop\" to see the items available for purchase.",
            };
            TwitchClientManager.SpoolMessageAsMe(message.Channel, player, outputMessage, Priority.Low);
        }

        public IShop AddItem(IItem item)
        {
            Items.Add(item);

            return this;
        }
    }
}
