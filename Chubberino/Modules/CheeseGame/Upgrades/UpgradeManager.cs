using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public sealed class UpgradeManager : IUpgradeManager
    {
        private static String StorageDescription { get; } = "+" + (Constants.StorageUpgradePercent * 100) + "% storage increase | +{0}% total";

        private static String QuestHelpDescription { get; } = "+" + (Constants.QuestBaseSuccessChance * Constants.QuestWorkerSuccessBonus * 100) + "% quest success per worker | +{0}% total";

        private static String ProductionDescription { get; } = "+" + (Constants.WorkerUpgradePercent * 100) + "% cheese per worker | +{0}% total";

        public Upgrade GetNextUpgradeToUnlock(Player player)
        {
            if (player.NextWorkerQuestSuccessUpgradeUnlock > player.NextStorageUpgradeUnlock)
            {
                // Storage
                return new Upgrade(
                    String.Format(StorageDescription, (Int32)(player.NextStorageUpgradeUnlock + 1) * Constants.StorageUpgradePercent * 100),
                    player.NextStorageUpgradeUnlock,
                    (Int32)(200 + Math.Max(1.5, (Int32)player.NextStorageUpgradeUnlock) * 100),
                    x => x.NextStorageUpgradeUnlock++);

            }
            else if (player.NextWorkerProductionUpgradeUnlock > player.NextWorkerQuestSuccessUpgradeUnlock)
            {
                // Quest success
                return new Upgrade(
                    String.Format(QuestHelpDescription, (Int32)(player.NextWorkerQuestSuccessUpgradeUnlock + 1) * Constants.QuestBaseSuccessChance * Constants.QuestWorkerSuccessBonus),
                    player.NextWorkerQuestSuccessUpgradeUnlock,
                    (Int32)(50 + Math.Pow(1.5, (Int32)player.NextWorkerQuestSuccessUpgradeUnlock) * 100),
                    x => x.NextWorkerQuestSuccessUpgradeUnlock++);
            }
            else if (player.NextWorkerProductionUpgradeUnlock < Rankings.Rank.Legend)
            {
                // Worker production
                return new Upgrade(
                    String.Format(ProductionDescription, (Int32)(player.NextWorkerProductionUpgradeUnlock + 1) * Constants.WorkerUpgradePercent * 100),
                    player.NextWorkerProductionUpgradeUnlock,
                    (Int32)(100 + Math.Pow(2, (Int32)player.NextWorkerProductionUpgradeUnlock) * 100),
                    x => x.NextWorkerProductionUpgradeUnlock++);
            }
            else
            {
                return null;
            }
        }
    }
}
