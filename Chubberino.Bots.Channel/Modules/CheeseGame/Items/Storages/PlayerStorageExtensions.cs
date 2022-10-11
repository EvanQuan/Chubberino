using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Upgrades;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Storages;

public static class PlayerStorageExtensions
{
    public static Double GetStorageUpgradeMultiplier(this Player player)
        => 1 + (Int32)player.NextStorageUpgradeUnlock * RankUpgradeExtensions.StorageUpgradePercent;

    public static Int32 GetTotalStorage(this Player player)
        => (Int32)(player.MaximumPointStorage * player.GetStorageUpgradeMultiplier());
}
