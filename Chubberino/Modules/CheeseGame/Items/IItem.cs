using Chubberino.Modules.CheeseGame.Models;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Items
{
    public interface IItem
    {
        IEnumerable<String> Names { get; }

        Int32 GetPrice(Player player);

        Either<BuyResult, String> TryBuy(Int32 quantity, Player player);

        String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity);
    }
}
