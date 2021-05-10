using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Models;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items
{
    public sealed class IncrementingPriceItem : Item
    {
        public const Int32 InitialPrice = 2;

        public Int32 Price { get; set; } = InitialPrice;

        public override IEnumerable<String> Names { get; } = new String[] { "IncrementingPriceItem", "s" };

        public override Option<String> TryBuySingle(Player player, Int32 price)
        {
            player.Points -= price;
            return Option.Nothing<String>();
        }

        public override String GetSpecificName(Player player)
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
