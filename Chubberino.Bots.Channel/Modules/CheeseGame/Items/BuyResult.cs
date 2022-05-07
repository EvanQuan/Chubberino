using System;

namespace Chubberino.Modules.CheeseGame.Items;

public readonly record struct BuyResult(Int32 QuantityPurchased, Int32 PointsSpent, String ExtraMessage);
