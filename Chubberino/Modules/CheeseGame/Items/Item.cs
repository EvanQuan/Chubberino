using Chubberino.Modules.CheeseGame.Models;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Items
{
    public abstract class Item : IItem
    {
        public abstract IEnumerable<String> Names { get; }


        public const String NotEnoughPointsErrorMessage = "You need {0} more cheese to buy {1}.";

        public Either<BuyResult, String> TryBuy(Int32 quantity, Player player)
        {
            Int32 quantityPurchased = 0;
            Int32 unitQuantityPurchased = 0;
            Int32 pointsSpent = 0;
            Int32 currentPrice = GetPrice(player);

            String errorMessage = null;

            // Buy item 1 at a time to account for changing prices for each additional purchase.
            Boolean shouldContinue = false;
            do
            {
                if (player.Points >= currentPrice)
                {
                    EitherPair<Int32, String> result = TryBuySingleUnit(player, currentPrice)();

                    if (result.IsRight)
                    {
                        // Could not buy due to some item-specific non-price restriction.
                        errorMessage = result.Right;
                        shouldContinue = false;
                    }
                    else
                    {
                        quantityPurchased += result.Left;
                        unitQuantityPurchased++;
                        pointsSpent += currentPrice;
                        shouldContinue = unitQuantityPurchased < quantity;
                        currentPrice = GetPrice(player);
                    }
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

            return errorMessage == null
                ? () => new BuyResult(quantityPurchased, pointsSpent)
                : () => errorMessage;
        }

        /// <summary>
        /// Try buying a single unit
        /// </summary>
        /// <param name="player"></param>
        /// <param name="price"></param>
        /// <returns>Left, the quantity purchased; or right, the error message.</returns>
        public abstract Either<Int32, String> TryBuySingleUnit(Player player, Int32 price);

        /// <summary>
        /// When not able to buy due to lack of points, the error message
        /// requires a name for the item. This is specific to the particular
        /// item being purchased, usually in the base quantity.
        /// unit
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public abstract String GetSpecificNameForNotEnoughToBuy(Player player);

        /// <summary>
        /// Get the current price based on the <paramref name="priceDeterminer"/>.
        /// </summary>
        /// <param name="priceDeterminer">Factor that influences the current price.</param>
        /// <returns>Current price</returns>
        public abstract Int32 PriceFunction(Int32 priceDeterminer);

        /// <summary>
        /// Get the <see cref="PriceFunction(Int32)"/> determiner.
        /// </summary>
        /// <param name="player">Player to get determiner from.</param>
        /// <returns>Price function determiner.</returns>
        public abstract Int32 GetPriceFunctionDeterminer(Player player);

        public Int32 GetPrice(Player player)
        {
            return PriceFunction(GetPriceFunctionDeterminer(player));
        }

        public abstract String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity);
    }
}
