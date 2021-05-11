using System;

namespace Chubberino.Modules.CheeseGame.Items
{
    public struct BuyResult
    {
        public BuyResult(Int32 quantityPurchased, Int32 pointsSpent)
        {
            QuantityPurchased = quantityPurchased;
            PointsSpent = pointsSpent;
        }

        public Int32 QuantityPurchased { get; }

        public Int32 PointsSpent { get; }
    }
}