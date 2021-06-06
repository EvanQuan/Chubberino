using Chubberino.Client;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Shops
{
    public class Shop : IShop
    {
        public IList<IItem> Items { get; }
        public IApplicationContextFactory ContextFactory { get; }
        public ITwitchClientManager Client { get; }
        public Random Random { get; }
        public IEmoteManager EmoteManager { get; }

        public Shop(
            IApplicationContextFactory contextFactory,
            ITwitchClientManager client,
            Random random,
            IEmoteManager emoteManager)
        {
            Items = new List<IItem>();
            ContextFactory = contextFactory;
            Client = client;
            Random = random;
            EmoteManager = emoteManager;
        }

        public void ListInventory(ChatMessage message)
        {
            using var context = ContextFactory.GetContext();

            Player player = context.GetPlayer(Client, message);

            StringBuilder inventoryPrompt = new();

            foreach (var item in Items)
            {
                var prompt = item.GetShopPrompt(player);

                if (prompt != null)
                {
                    inventoryPrompt
                        .Append(" | ")
                        .Append(prompt);
                }
            }

            Client.SpoolMessageAsMe(message.Channel, player, inventoryPrompt.ToString(), Priority.Low);
        }

        public void BuyItem(ChatMessage message)
        {
            // Cut out "!cheese buy " or "!cheese b" start.
            String arguments = message.Message
                .GetNextWord(out _)
                .GetNextWord(out _);

            using var context = ContextFactory.GetContext();

            Player player = context.GetPlayer(Client, message);

            if (String.IsNullOrWhiteSpace(arguments))
            {
                Client.SpoolMessageAsMe(message.Channel, player, $"Please enter an item to buy with \"!cheese buy <name of item>\". Type \"!cheese shop\" to see the items available for purchase.", Priority.Low);
                return;
            }

            // Cut out space between buy and item
            String remainingArguments = arguments.GetNextWord(out String itemToBuy);
            
            String outputMessage = GetBuyItemMessage(message, context, player, remainingArguments, itemToBuy, out var priority);

            Client.SpoolMessageAsMe(message.Channel, player, outputMessage, priority);
        }

        private String GetBuyItemMessage(ChatMessage message, IApplicationContext context, Player player, String remainingArguments, String itemToBuy, out Priority priority)
        {
            priority = Priority.Low;
            if (Items.TryGetFirst(x => x.Names.Contains(itemToBuy, StringComparer.InvariantCultureIgnoreCase), out var item))
            {
                remainingArguments.GetNextWord(out String quantityString);

                Int32 quantityRequested = Int32.TryParse(quantityString, out Int32 quantityParsed) && quantityParsed > 0
                    ? quantityParsed
                    : new String[] { "a", "all" }.Contains(quantityString, StringComparer.InvariantCultureIgnoreCase)
                        ? Int32.MaxValue
                        : 1;

                var result = item.TryBuy(quantityRequested, player)();
                    
                if (result.IsRight)
                {
                    return result.Right;
                }

                var buyResult = result.Left;

                var outputMessage = $"You bought {item.GetSpecificNameForSuccessfulBuy(player, buyResult.QuantityPurchased)}. " +
                    $"{buyResult.ExtraMessage} " +
                    $"{Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Positive))} " +
                    $"(-{buyResult.PointsSpent} cheese)";

                priority = Priority.Medium;

                context.SaveChanges();

                return outputMessage;
            }

            return $"Invalid item \"{itemToBuy}\" to buy. Type \"!cheese shop\" to see the items available for purchase.";
        }

        public IShop AddItem(IItem item)
        {
            Items.Add(item);

            return this;
        }
    }
}
