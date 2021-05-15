using Chubberino.Modules.CheeseGame.Models;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Items
{
    public sealed class Mousetrap : Item
    {
        public override IEnumerable<String> Names { get; } = new String[] { "Mousetrap", "m", "mouse", "mousetraps" };

        public override Int32 GetPrice(Player player)
        {
            return player.GetMousetrapPrice();
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            return "a mousetrap";
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            return $"{quantity} mousetrap{(quantity == 1 ? String.Empty : "s")}";
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            player.MouseTrapCount++;
            player.Points -= price;

            return () => 1;
        }

        public override String GetShopPrompt(Player player)
        {
            return $"{base.GetShopPrompt(player)} [+1] for {GetPrice(player)} cheese";
        }
    }
}
