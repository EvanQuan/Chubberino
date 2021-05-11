using Chubberino.Modules.CheeseGame.Models;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Items
{
    public sealed class Gear : Item
    {
        public override IEnumerable<String> Names => new String[] { "Gear", "g", "gears" };

        public override Int32 GetPrice(Player player)
        {
            return 5 + 5 * player.GearCount;
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            return "some gear";
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            return $"{quantity} gear";
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            player.GearCount++;
            player.Points -= price;

            return () => 1;
        }
    }
}
