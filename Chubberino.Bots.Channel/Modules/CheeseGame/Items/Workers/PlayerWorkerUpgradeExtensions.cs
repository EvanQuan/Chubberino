using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Workers;

public static class PlayerWorkerUpgradeExtensions
{
    public static Double GetWorkerPointMultiplier(this Player player)
        => player.NextWorkerProductionUpgradeUnlock.GetWorkerPointMultiplier();
}
