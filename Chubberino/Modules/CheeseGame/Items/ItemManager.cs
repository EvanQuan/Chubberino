﻿using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Items
{
    public sealed class ItemManager : IItemManager
    {
        public PriceList GetPrices(Player player)
        {
            return new PriceList(
                storage: 25 + player.MaximumPointStorage / 2,
                population: (Int32)(20 + Math.Pow(player.PopulationCount, 2)),
                worker: (Int32)(100 + 5 * Math.Pow(player.WorkerCount, 2)),
                mousetrap: 50);
        }
    }
}
