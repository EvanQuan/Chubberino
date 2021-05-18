using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Items.Workers
{
    public static class PlayerWorkerUpgradeExtensions
    {
        public static Double GetWorkerPointMultiplier(this Player player)
        {
            return player.NextWorkerProductionUpgradeUnlock.GetWorkerPointMultiplier();
        }
    }
}
