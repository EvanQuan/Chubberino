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

        public override Int32 GetPriceFunctionDeterminer(Player player)
        {
            return 0;
        }

        public override String GetSpecificName(Player player)
        {
            return "an Error Item";
        }

        public override Int32 PriceFunction(Int32 priceDeterminer)
        {
            return 0;
        }

        public override Option<String> TryBuySingle(Player player, Int32 price)
        {
            player.Points -= price;
            return () => ErrorMessage;
        }
    }
}
