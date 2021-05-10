using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Models;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items
{
    public sealed class StaticPriceItem : Item
    {
        public const Int32 StaticPrice = 2;

        public override IEnumerable<String> Names { get; } = new String[] { "StaticPriceItem", "s" };

        public override Int32 GetPriceFunctionDeterminer(Player player)
        {
            return 0;
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            return "a Static Price Item";
        }

        public override Int32 PriceFunction(Int32 priceDeterminer)
        {
            return StaticPrice;
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            player.Points -= price;
            return () => 1;
        }
    }
}
