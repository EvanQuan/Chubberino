using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Workers;

public static class RankWorkerUpgradeExtensions
{
    /// <summary>
    /// The additional worker point percent increase per upgrade.
    /// </summary>
    public const Double WorkerUpgradePercent = 0.02;

    /// <summary>
    /// The base point increase value workers provide.
    /// </summary>
    public const Double BaseWorkerPointPercent = 0.1;

    public static Double GetWorkerPointMultiplier(this Rank rank)
        => BaseWorkerPointPercent + (Int32)rank * WorkerUpgradePercent;
}
