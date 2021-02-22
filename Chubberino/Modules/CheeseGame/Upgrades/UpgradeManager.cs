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
            if (player.LastWorkerQuestHelpUnlocked > player.LastStorageUpgradeUnlocked)
            {
                // Storage
                return new Upgrade(
                    String.Format(StorageDescription, (Int32)(player.LastStorageUpgradeUnlocked + 1) * Constants.StorageUpgradePercent * 100),
                    player.LastStorageUpgradeUnlocked,
                    (Int32)(200 + Math.Max(1.5, (Int32)player.LastStorageUpgradeUnlocked) * 100),
                    x => x.LastStorageUpgradeUnlocked++);

            }
            else if (player.LastWorkerProductionUpgradeUnlocked > player.LastWorkerQuestHelpUnlocked)
            {
                // Quest success
                return new Upgrade(
                    String.Format(QuestHelpDescription, (Int32)(player.LastWorkerQuestHelpUnlocked + 1) * Constants.QuestBaseSuccessChance * Constants.QuestWorkerSuccessBonus),
                    player.LastWorkerQuestHelpUnlocked,
                    (Int32)(50 + Math.Pow(1.5, (Int32)player.LastWorkerQuestHelpUnlocked) * 100),
                    x => x.LastWorkerQuestHelpUnlocked++);
            }
            else if (player.LastWorkerProductionUpgradeUnlocked == Rankings.Rank.Legend)
            {
                return null;
            }
            else
            {
                // Worker production
                return new Upgrade(
                    String.Format(ProductionDescription, (Int32)(player.LastWorkerProductionUpgradeUnlocked + 1) * Constants.WorkerUpgradePercent * 100),
                    player.LastWorkerProductionUpgradeUnlocked,
                    (Int32)(100 + Math.Pow(2, (Int32)player.LastWorkerProductionUpgradeUnlocked) * 100),
                    x => x.LastWorkerProductionUpgradeUnlocked++);
            }
        }
    }
}
