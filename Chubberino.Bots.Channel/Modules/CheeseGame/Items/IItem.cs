using System;
using System.Collections.Generic;
using Chubberino.Database.Models;
using LanguageExt;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items;

public interface IItem
{
    IEnumerable<String> Names { get; }

    Int32 GetPrice(Player player);

    /// <summary>
    /// Try to buy a <paramref name="quantity"/> of this item for a specified <paramref name="player"/>.
    /// </summary>
    /// <param name="quantity">Quantity of item requested to buy.</param>
    /// <param name="player">Player to buy item for.</param>
    /// <returns>Left successful result of buying at least 1 item; right unsuccessful message explaining reason.</returns>
    Either<BuyResult, String> TryBuy(Int32 quantity, Player player);

    String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity);

    /// <summary>
    /// States if the item is currently for sale to the specified <paramref name="player"/>.
    /// If so, it will appear in the shop description.
    /// </summary>
    /// <param name="player">The specified player to check if the item is available.</param>
    /// <returns>true if the item is for sale; false otherwise.</returns>
    Boolean IsForSale(Player player, out String reason);

    /// <summary>
    /// Get the shop prompt description of the item for the specified <paramref name="player"/>.
    /// </summary>
    /// <param name="player">Player that the prompt is for.</param>
    /// <returns>Shop prompt</returns>
    String GetShopPrompt(Player player);
}
