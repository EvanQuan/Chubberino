using Chubberino.Database.Models;
using System;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Workers;

public static class PlayerWorkerUpgradeExtensions
{
    public static Double GetWorkerPointMultiplier(this Player player)
    {
        return player.NextWorkerProductionUpgradeUnlock.GetWorkerPointMultiplier();
    }
}
