using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Items
{
    public interface IItemManager
    {
        PriceList GetPrices(Player player);
    }
}
