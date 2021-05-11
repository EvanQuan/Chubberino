using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Models;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items
{
    public sealed class ErrorItem : Item
    {
        public const String ErrorMessage = "a";

        public override IEnumerable<String> Names { get; } = new String[] { "ErrorItem", "e" };

        public override Int32 GetPrice(Player player)
        {
            return 0;
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            return "an Error Item";
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            return quantity == 1 ? "Error Item" : "Error Items";
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            player.Points -= price;
            return () => ErrorMessage;
        }
    }
}
