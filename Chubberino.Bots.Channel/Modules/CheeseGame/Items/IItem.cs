using System.Collections.Generic;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items;

public interface IItem
{
    IEnumerable<String> Names { get; }

    /// <summary>
    /// Get the current price based on the specified <paramref name="player"/>.
    /// </summary>
    /// <param name="player">Player to get the price for.</param>
    /// <returns>
    /// <see cref="Int32"/> current price of the item;
    /// <see cref="String"/> unsuccessful message explaining reason.
    /// </returns>
    Either<Int32, String> GetPrice(Player player);

    /// <summary>
    /// Try to buy a <paramref name="quantity"/> of this item for a specified <paramref name="player"/>.
    /// </summary>
    /// <param name="quantity">Quantity of item requested to buy.</param>
    /// <param name="player">Player to buy item for.</param>
    /// <returns>
    /// <see cref="BuyResult"/> successful result of buying at least 1 item;
    /// <see cref="String"/> unsuccessful message explaining reason.
    /// </returns>
    Either<BuyResult, String> TryBuy(Int32 quantity, Player player);

    String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity);

    /// <summary>
    /// States if the item is currently for sale to the specified <paramref name="player"/>.
    /// If so, it will appear in the shop description.
    /// </summary>
    /// <param name="player">The specified player to check if the item is available.</param>
    /// <returns>
    /// <see cref="String"/> reason if if the item is not for sale;
    /// <see cref="Option{A}.None"/> otherwise.
    /// </returns>
    Option<String> IsForSale(Player player);

    /// <summary>
    /// Get the shop prompt description of the item for the specified <paramref name="player"/>.
    /// </summary>
    /// <param name="player">Player that the prompt is for.</param>
    /// <returns>
    /// <see cref="String"/> if the item has a prompt to display;
    /// <see cref="Option{A}.None"/> otherwise.</returns>
    Option<String> GetShopPrompt(Player player);
}
