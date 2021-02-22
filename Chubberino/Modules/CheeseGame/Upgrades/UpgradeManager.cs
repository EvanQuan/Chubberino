using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public sealed class UpgradeManager : IUpgradeManager
    {
        private const String StorageDescription = "+{0}% -> +{1}% storage increase";

        private const String QuestSuccessDescription = "+{0}% -> +{1}% quest success chance";

        private const String ProductionDescription = "+{0}% -> +{1}% cheese per worker";

        private const String CriticalCheeseDescription = "+{0}% -> +{1}% critical cheese chance";

        public Upgrade GetNextUpgradeToUnlock(Player player)
        {
            if (player.NextQuestSuccessUpgradeUnlock > player.NextStorageUpgradeUnlock)
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
            else if (player.NextQuestSuccessUpgradeUnlock > player.NextCriticalCheeseUpgradeUnlock)
            {
                // Critical cheese
                Double currentUpgradePercent = (Int32)(player.NextCriticalCheeseUpgradeUnlock) * Constants.CriticalCheeseUpgradePercent * 100;
                Double nextUpgradePercent = (Int32)(player.NextCriticalCheeseUpgradeUnlock + 1) * Constants.CriticalCheeseUpgradePercent * 100;
                return new Upgrade(
                    String.Format(CriticalCheeseDescription, currentUpgradePercent, nextUpgradePercent),
                    player.NextCriticalCheeseUpgradeUnlock,
                    (Int32)(Math.Pow(1.5, (Int32)player.NextQuestSuccessUpgradeUnlock) * 50),
                    x => x.NextCriticalCheeseUpgradeUnlock++);
            }
            else if (player.NextWorkerProductionUpgradeUnlock > player.NextQuestSuccessUpgradeUnlock)
            {
                // Worker quest success
                Double currentUpgradePercent = player.GetQuestSuccessChance() * 100;
                Double nextUpgradePercent = currentUpgradePercent + Constants.QuestSuccessUpgradePercent * 100;
                return new Upgrade(
                    String.Format(QuestSuccessDescription, currentUpgradePercent, nextUpgradePercent),
                    player.NextQuestSuccessUpgradeUnlock,
                    (Int32)(50 + Math.Pow(1.5, (Int32)player.NextQuestSuccessUpgradeUnlock) * 100),
                    x => x.NextQuestSuccessUpgradeUnlock++);
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
