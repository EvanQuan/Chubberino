﻿using Chubberino.Bots.Channel.Modules.CheeseGame.Emotes;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items;
using Chubberino.Common.Extensions;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Shops;

public sealed class Shop : IShop
{
    public IItem[] Items { get; set; } = Array.Empty<Item>();
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
        ContextFactory = contextFactory;
        Client = client;
        Random = random;
        EmoteManager = emoteManager;
    }

    public void ListInventory(ChatMessage message)
    {
        using var context = ContextFactory.GetContext();

        Player player = context.GetPlayer(Client, message);

        var itemDescriptions = Items
            .SelectMany(item => item.GetShopPrompt(player));

        var inventoryPrompt = String.Join(" | ", itemDescriptions);

        Client.SpoolMessageAsMe(message.Channel, player, inventoryPrompt, Priority.Low);
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

        String outputMessage = GetBuyItemMessage(message, context, player, remainingArguments, itemToBuy);

        Client.SpoolMessageAsMe(message.Channel, player, outputMessage, Priority.Low);
    }

    private String GetBuyItemMessage(
        ChatMessage message,
        IApplicationContext context,
        Player player,
        String remainingArguments,
        String itemToBuy)
        => Items
            .TryGetFirst(x => x.Names.Contains(itemToBuy, StringComparer.InvariantCultureIgnoreCase))
            .Some(item =>
            {
                remainingArguments.GetNextWord(out String quantityString);

                Int32 quantityRequested = Int32.TryParse(quantityString, out Int32 quantityParsed) && quantityParsed > 0
                    ? quantityParsed
                    : new String[] { "a", "all" }.Contains(quantityString, StringComparer.InvariantCultureIgnoreCase)
                        ? Int32.MaxValue
                        : 1;

                var result = item.TryBuy(quantityRequested, player);

                return result
                    .Right(error => error)
                    .Left(buyResult =>
                    {
                        var outputMessage = $"You bought {item.GetSpecificNameForSuccessfulBuy(player, buyResult.QuantityPurchased)}. " +
                            $"{buyResult.ExtraMessage} " +
                            $"{Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Positive))} " +
                            $"(-{buyResult.PointsSpent} cheese)";

                        context.SaveChanges();

                        return outputMessage;
                    });
            })
            .None(() => $"Invalid item \"{itemToBuy}\" to buy. Type \"!cheese shop\" to see the items available for purchase.");
}
