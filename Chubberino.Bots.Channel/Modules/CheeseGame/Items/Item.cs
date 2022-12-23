using System.Collections.Generic;
using Chubberino.Common.Extensions;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items;

public abstract class Item : IItem
{
    public const String NotEnoughPointsErrorMessage = "You need {0} more cheese to buy {1}.";

    public const String UnexpectedErrorMessage = "This error message should never show up. monkaX ";

    public abstract IEnumerable<String> Names { get; }


    public Either<BuyResult, String> TryBuy(Int32 quantity, Player player) =>
        // Check if the item is for sale.
        // Items that are not for sale are still visible in the shop, but
        // have a special reason why they cannot be purchased when
        // attempting to do so.
        IsForSale(player)
            .Some(reason => Either<BuyResult, String>.Right(reason))
            .None(() =>
            {
                OnBeforeBuy();

                return GetPrice(player)
                    .Right(error => Either<BuyResult, String>.Right(error))
                    .Left(initialPrice =>
                    {
                        String errorMessage = null;

                        // Buy item 1 at a time to account for changing prices for each additional purchase.
                        Boolean shouldContinue = false;

                        Int32 quantityPurchased = 0;
                        Int32 pointsSpent = 0;
                        var currentPrice = initialPrice;
                        Int32 unitQuantityPurchased = 0;
                        do
                        {
                            if (player.Points >= currentPrice)
                            {
                                Either<Int32, String> result = TryBuySingleUnit(player, currentPrice);

                                result
                                    .Right(error =>
                                    {
                                        // Could not buy due to some item-specific non-price restriction.
                                        errorMessage = error;
                                        shouldContinue = false;
                                    })
                                    .Left(count =>
                                    {
                                        // Successfully purchased count quantity.
                                        quantityPurchased += count;
                                        unitQuantityPurchased++;
                                        pointsSpent += currentPrice;
                                        shouldContinue = unitQuantityPurchased < quantity;
                                        GetPrice(player)
                                            .Right(error =>
                                            {
                                                // Could not buy due to some item-specific price restriction.
                                                // No error because we succesfully purchased some quanity.
                                                shouldContinue = false;
                                            })
                                            .Left(price => currentPrice = price);
                                    });
                            }
                            else if (unitQuantityPurchased == 0)
                            {
                                // Don't have enough to by even 1 unit.
                                errorMessage = String.Format(NotEnoughPointsErrorMessage, currentPrice - player.Points, GetSpecificNameForNotEnoughToBuy(player));
                                shouldContinue = false;
                            }
                            else
                            {
                                // Requested more than can buy.
                                // Return a successful purchase, just buying the maximum
                                shouldContinue = false;
                            }
                        }
                        while (shouldContinue);

                        var extraMessage = OnAfterBuy(player, quantityPurchased, pointsSpent);

                        return errorMessage is null
                            ? new BuyResult(quantityPurchased, pointsSpent, extraMessage)
                            : errorMessage;
                    });
            });

    /// <summary>
    /// Try buying a single unit
    /// </summary>
    /// <param name="player"></param>
    /// <param name="price"></param>
    /// <returns>Left, the quantity purchased; or right, the error message.</returns>
    public abstract Either<Int32, String> TryBuySingleUnit(Player player, Int32 price);

    /// <summary>
    /// Do stuff before buying any of the item. This is called when buying is valid (after the is for sale check)
    /// </summary>
    public virtual void OnBeforeBuy() { }

    /// <summary>
    /// Do stuff after buying all quantities of the item.
    /// </summary>
    public virtual String OnAfterBuy(Player player, Int32 quantityPurchased, Int32 pointsSpent) => String.Empty;

    /// <summary>
    /// When not able to buy due to lack of points, the error message
    /// requires a name for the item. This is specific to the particular
    /// item being purchased, usually in the base quantity.
    /// unit
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public abstract String GetSpecificNameForNotEnoughToBuy(Player player);

    public abstract Either<Int32, String> GetPrice(Player player);
    public String GetPriceString(Player player) => GetPrice(player)
        .Right(error => error)
        .Left(price => price.ToString());

    public abstract String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity);

    public virtual Option<String> IsForSale(Player player) => Option<String>.None;

    public virtual Option<String> GetShopPrompt(Player player) => Names.TryGetFirst();
    protected String GetBaseShopPrompt(Player player) => Names.First();
}