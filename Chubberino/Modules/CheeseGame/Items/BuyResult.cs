using System;

namespace Chubberino.Modules.CheeseGame.Items
{
    public struct BuyResult
    {
        public BuyResult(Int32 quantityPurchased, Int32 pointsSpent, String extraMessage)
        {
            QuantityPurchased = quantityPurchased;
            PointsSpent = pointsSpent;
            ExtraMessage = extraMessage;
        }

        public Int32 QuantityPurchased { get; }

        public Int32 PointsSpent { get; }

        public String ExtraMessage { get; }
    }
}