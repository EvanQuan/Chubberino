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
            Int32 pointsSpent = 0;
            Int32 currentPrice = GetPrice(player);

            String errorMessage = null;

            // Buy item 1 at a time to account for changing prices for each additional purchase.
            Boolean shouldContinue = false;
            do
            {
                if (player.Points >= currentPrice)
                {
                    var error = TryBuySingle(player, currentPrice);

                    if (error.HasValue())
                    {
                        errorMessage = error.Value();
                        shouldContinue = false;
                    }
                    else
                    {
                        quantityPurchased++;
                        pointsSpent += currentPrice;
                        shouldContinue = quantityPurchased < quantity;
                        currentPrice = GetPrice(player);
                    }
                }
                else if (quantityPurchased == 0)
                {
                    errorMessage = String.Format(NotEnoughPointsErrorMessage, currentPrice - player.Points, GetSpecificName(player));
                }
            }
            while (shouldContinue);

            return errorMessage == null
                ? () => new BuyResult(quantityPurchased, pointsSpent)
                : () => errorMessage;
        }

        public abstract Option<String> TryBuySingle(Player player, Int32 price);

        public abstract String GetSpecificName(Player player);

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
    }
}
