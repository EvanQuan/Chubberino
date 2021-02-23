using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public static class PlayerUpgradeExtensions
    {
        private const String StorageDescription = "+{0}% -> +{1}% storage increase";

        private const String QuestSuccessDescription = "{0}% -> {1}% quest success chance";

        private const String ProductionDescription = "+{0}% -> +{1}% cheese per worker";

        private const String CriticalCheeseDescription = "{0}% -> {1}% critical cheese chance";

        public static Upgrade GetNextStorageUpgrade(this Player player)
        {
            Double currentUpgradePercent = (Int32)(player.NextStorageUpgradeUnlock) * Constants.StorageUpgradePercent * 100;
            Double nextUpgradePercent = (Int32)(player.NextStorageUpgradeUnlock + 1) * Constants.StorageUpgradePercent * 100;
            return new Upgrade(
                String.Format(StorageDescription, currentUpgradePercent, nextUpgradePercent),
                player.NextStorageUpgradeUnlock,
                (Int32)(200 + Math.Max(1.5, (Int32)player.NextStorageUpgradeUnlock) * 100),
                x => x.NextStorageUpgradeUnlock++);
        }

        public static Upgrade GetNextCriticalCheeseUpgrade(this Player player)
        {
            Double currentUpgradePercent = (Int32)(player.NextCriticalCheeseUpgradeUnlock) * Constants.CriticalCheeseUpgradePercent * 100;
            Double nextUpgradePercent = (Int32)(player.NextCriticalCheeseUpgradeUnlock + 1) * Constants.CriticalCheeseUpgradePercent * 100;
            return new Upgrade(
                String.Format(CriticalCheeseDescription, currentUpgradePercent, nextUpgradePercent),
                player.NextCriticalCheeseUpgradeUnlock,
                50 + (Int32)(Math.Pow(1.5, (Int32)player.NextCriticalCheeseUpgradeUnlock) * 75),
                x => x.NextCriticalCheeseUpgradeUnlock++);
        }

        public static Upgrade GetNextQuestSuccessUpgrade(this Player player)
        {
            Int32 currentUpgradePercent = (Int32)(player.GetQuestSuccessChance() * 100);
            Int32 nextUpgradePercent = currentUpgradePercent + (Int32)(Constants.QuestSuccessUpgradePercent * 100);
            return new Upgrade(
                String.Format(QuestSuccessDescription, currentUpgradePercent, nextUpgradePercent),
                player.NextQuestSuccessUpgradeUnlock,
                (Int32)(50 + Math.Pow(1.5, (Int32)player.NextQuestSuccessUpgradeUnlock) * 80),
                x => x.NextQuestSuccessUpgradeUnlock++);
        }

        public static Upgrade GetNextWorkerProductionUpgrade(this Player player)
        {
            Int32 currentUpgradePercent = (Int32)((Int32)(player.NextWorkerProductionUpgradeUnlock + 1) * Constants.WorkerUpgradePercent * 100);
            Int32 nextUpgradePercent = (Int32)((Int32)(player.NextWorkerProductionUpgradeUnlock + 2) * Constants.WorkerUpgradePercent * 100);
            return new Upgrade(
                String.Format(ProductionDescription, currentUpgradePercent, nextUpgradePercent),
                player.NextWorkerProductionUpgradeUnlock,
                (Int32)(100 + Math.Pow(2, (Int32)player.NextWorkerProductionUpgradeUnlock) * 100),
                x => x.NextWorkerProductionUpgradeUnlock++);
        }

        public static Upgrade GetNextUpgrade(this Player player)
        {
            if (player.NextCriticalCheeseUpgradeUnlock > player.NextStorageUpgradeUnlock)
            {
                return player.GetNextStorageUpgrade();
            }
            else if (player.NextQuestSuccessUpgradeUnlock > player.NextCriticalCheeseUpgradeUnlock)
            {
                return player.GetNextCriticalCheeseUpgrade();
            }
            else if (player.NextWorkerProductionUpgradeUnlock > player.NextQuestSuccessUpgradeUnlock)
            {
                return player.GetNextQuestSuccessUpgrade();
            }
            else if (player.NextWorkerProductionUpgradeUnlock < Rankings.Rank.Legend)
            {
                return player.GetNextWorkerProductionUpgrade();
            }
            else
            {
                return null;
            }
        }
    }
}
