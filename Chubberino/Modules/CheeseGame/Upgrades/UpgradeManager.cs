using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public sealed class UpgradeManager : IUpgradeManager
    {
        private const String StorageDescription = "+{0}% -> +{1}% storage increase";

        private const String QuestHelpDescription = "+{0}% -> +{1}% quest success per worker";

        private const String ProductionDescription = "+{0}% -> +{1}% cheese per worker";

        private const String CriticalCheeseDescription = "+{0}% -> +{1}% critical cheese chance";

        public Upgrade GetNextUpgradeToUnlock(Player player)
        {
            if (player.NextWorkerQuestSuccessUpgradeUnlock > player.NextStorageUpgradeUnlock)
            {
                // Storage
                Double currentUpgradePercent = (Int32)(player.NextStorageUpgradeUnlock) * Constants.StorageUpgradePercent * 100;
                Double nextUpgradePercent = (Int32)(player.NextStorageUpgradeUnlock + 1) * Constants.StorageUpgradePercent * 100;
                return new Upgrade(
                    String.Format(StorageDescription, currentUpgradePercent, nextUpgradePercent),
                    player.NextStorageUpgradeUnlock,
                    (Int32)(200 + Math.Max(1.5, (Int32)player.NextStorageUpgradeUnlock) * 100),
                    x => x.NextStorageUpgradeUnlock++);

            }
            else if (player.NextWorkerQuestSuccessUpgradeUnlock > player.NextCriticalCheeseUpgradeUnlock)
            {
                // Critical cheese
                Double currentUpgradePercent = (Int32)(player.NextCriticalCheeseUpgradeUnlock) * Constants.CriticalCheeseUpgradePercent * 100;
                Double nextUpgradePercent = (Int32)(player.NextCriticalCheeseUpgradeUnlock + 1) * Constants.CriticalCheeseUpgradePercent * 100;
                return new Upgrade(
                    String.Format(CriticalCheeseDescription, currentUpgradePercent, nextUpgradePercent),
                    player.NextCriticalCheeseUpgradeUnlock,
                    (Int32)(50 + Math.Pow(1.5, (Int32)player.NextWorkerQuestSuccessUpgradeUnlock) * 50),
                    x => x.NextCriticalCheeseUpgradeUnlock++);
            }
            else if (player.NextWorkerProductionUpgradeUnlock > player.NextWorkerQuestSuccessUpgradeUnlock)
            {
                // Worker quest success
                Double currentUpgradePercent = (Int32)(player.NextWorkerQuestSuccessUpgradeUnlock) * Constants.QuestBaseSuccessChance * Constants.QuestWorkerSuccessBonus;
                Double nextUpgradePercent = (Int32)(player.NextWorkerQuestSuccessUpgradeUnlock + 1) * Constants.QuestBaseSuccessChance * Constants.QuestWorkerSuccessBonus;
                return new Upgrade(
                    String.Format(QuestHelpDescription, currentUpgradePercent, nextUpgradePercent),
                    player.NextWorkerQuestSuccessUpgradeUnlock,
                    (Int32)(50 + Math.Pow(1.5, (Int32)player.NextWorkerQuestSuccessUpgradeUnlock) * 100),
                    x => x.NextWorkerQuestSuccessUpgradeUnlock++);
            }
            else if (player.NextWorkerProductionUpgradeUnlock < Rankings.Rank.Legend)
            {
                // Worker production
                Double currentUpgradePercent = (Int32)(player.NextWorkerProductionUpgradeUnlock) * Constants.WorkerUpgradePercent * 100;
                Double nextUpgradePercent = (Int32)(player.NextWorkerProductionUpgradeUnlock + 1) * Constants.WorkerUpgradePercent * 100;
                return new Upgrade(
                    String.Format(ProductionDescription, currentUpgradePercent, nextUpgradePercent),
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
