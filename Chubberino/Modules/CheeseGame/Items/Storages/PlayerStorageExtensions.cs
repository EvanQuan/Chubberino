using Chubberino.Modules.CheeseGame.Items.Upgrades;
using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Items
{
    public static class PlayerStorageExtensions
    {
        public static Double GetStorageUpgradeMultiplier(this Player player)
        {
            return 1 + (Int32)player.NextStorageUpgradeUnlock * RankUpgradeExtensions.StorageUpgradePercent;
        }

        public static Int32 GetTotalStorage(this Player player)
        {
            return (Int32)(player.MaximumPointStorage * player.GetStorageUpgradeMultiplier());
        }
    }
}
