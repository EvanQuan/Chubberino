﻿using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Models;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items
{
    public sealed class IncrementingPriceItem : Item
    {
        public const Int32 InitialPrice = 2;

        public Int32 Price { get; private set; } = InitialPrice;

        public override IEnumerable<String> Names { get; } = new String[] { "IncrementingPriceItem", "s" };

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            player.Points -= price;
            return () => 1;
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            return "an Incrementing Price Item";
        }

        public override Int32 PriceFunction(Int32 priceDeterminer)
        {
            return Price++;
        }

        public override Int32 GetPriceFunctionDeterminer(Player player)
        {
            return 0;
        }
    }
}