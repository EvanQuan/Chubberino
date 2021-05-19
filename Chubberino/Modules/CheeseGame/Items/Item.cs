using Chubberino.Modules.CheeseGame.Models;
using Monad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Modules.CheeseGame.Items
{
    public abstract class Item : IItem
    {
        public const String NotEnoughPointsErrorMessage = "You need {0} more cheese to buy {1}.";

        public const String UnexpectedErrorMessage = "This error message should never show up.";

        public abstract IEnumerable<String> Names { get; }


        public Either<BuyResult, String> TryBuy(Int32 quantity, Player player)
        {
            // Check if the item is for sale.
            // Items that are not for sale are still visible in the shop, but
            // have a special reason why they cannot be purchased when
            // attempting to do so.
            if (!IsForSale(player, out String reason))
            {
                return () => reason;
            }

            OnBeforeBuy();

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

            var extraMessage = OnAfterBuy(player, quantityPurchased, pointsSpent);

            return errorMessage == null
                ? () => new BuyResult(quantityPurchased, pointsSpent, extraMessage)
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
        /// Do stuff before buying any of the item. This is called when buying is valid (after the is for sale check)
        /// </summary>
        public virtual void OnBeforeBuy() { }

        /// <summary>
        /// Do stuff after buying all quantities of the item.
        /// </summary>
        public virtual String OnAfterBuy(Player player, Int32 quantityPurchased, Int32 pointsSpent) { return String.Empty; }

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
        /// Get the current price based on the specified <paramref name="player"/>.
        /// </summary>
        /// <param name="player">Player to get the price for.</param>
        /// <returns>Current price</returns>
        public abstract Int32 GetPrice(Player player);

        public abstract String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity);

        public virtual Boolean IsForSale(Player player, out String reason)
        {
            reason = default;
            return true;
        }

        public virtual String GetShopPrompt(Player player)
        {
            return $"{Names.First()}";
        }
    }
}
