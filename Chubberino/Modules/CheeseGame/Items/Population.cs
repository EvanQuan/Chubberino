using Chubberino.Modules.CheeseGame.Models;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Items
{
    public sealed class Population : Item
    {
        public const Int32 ShopUnitQuantity = 5;

        public override IEnumerable<String> Names { get; } = new String[] { "Population", "p", "pop" };

        public override Int32 GetPrice(Player player)
        {
            return (Int32)(20 + Math.Pow(player.PopulationCount, 2));
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            return ShopUnitQuantity + " population slots";
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            return quantity + " population slots";
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            player.PopulationCount += ShopUnitQuantity;
            player.Points -= price;

            return () => ShopUnitQuantity;
        }
    }
}
