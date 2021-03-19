using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Utility;
using System;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public sealed class UpgradeManager : IUpgradeManager
    {
        private const String StorageDescription = "+{0}% -> +{1}% storage increase";

        private const String QuestRewardDescription = "{0}% -> {1}% cheese quest reward increase";

        private const String ProductionDescription = "+{0}% -> +{1}% cheese per worker";

        private const String CriticalCheeseDescription = "{0}% -> {1}% critical cheese chance";

        public ICalculator Calculator { get; }

        public UpgradeManager(ICalculator calculator)
        {
            Calculator = calculator;
        }

        public Upgrade GetNextUpgradeToUnlock(Player player)
        {
            return GetNextUpgrade(player);
        }

        public static Upgrade GetNextStorageUpgrade(Player player)
        {
            Double currentUpgradePercent = (Int32)(player.NextStorageUpgradeUnlock) * Constants.StorageUpgradePercent * 100;
            Double nextUpgradePercent = (Int32)(player.NextStorageUpgradeUnlock + 1) * Constants.StorageUpgradePercent * 100;
            return new Upgrade(
                String.Format(StorageDescription, currentUpgradePercent, nextUpgradePercent),
                player.NextStorageUpgradeUnlock,
                (Int32)(200 + Math.Max(1.5, (Int32)player.NextStorageUpgradeUnlock) * 100),
                x => x.NextStorageUpgradeUnlock++);
        }

        public static Upgrade GetNextCriticalCheeseUpgrade(Player player)
        {
            Double currentUpgradePercent = (Int32)(player.NextCriticalCheeseUpgradeUnlock) * Constants.CriticalCheeseUpgradePercent * 100;
            Double nextUpgradePercent = (Int32)(player.NextCriticalCheeseUpgradeUnlock + 1) * Constants.CriticalCheeseUpgradePercent * 100;
            return new Upgrade(
                String.Format(CriticalCheeseDescription, currentUpgradePercent, nextUpgradePercent),
                player.NextCriticalCheeseUpgradeUnlock,
                50 + (Int32)(Math.Pow(1.5, (Int32)player.NextCriticalCheeseUpgradeUnlock) * 75),
                x => x.NextCriticalCheeseUpgradeUnlock++);
        }

        public Upgrade GetNextQuestRewardUpgrade(Player player)
        {
            Int32 currentUpgradePercent = (Int32)(Calculator.GetQuestRewardMultiplier(player.NextQuestRewardUpgradeUnlock) * 100);
            Int32 nextUpgradePercent = (Int32)(Calculator.GetQuestRewardMultiplier(player.NextQuestRewardUpgradeUnlock.Next()) * 100);
            return new Upgrade(
                String.Format(QuestRewardDescription, currentUpgradePercent, nextUpgradePercent),
                player.NextQuestRewardUpgradeUnlock,
                (Int32)(50 + Math.Pow(1.5, (Int32)player.NextQuestRewardUpgradeUnlock) * 80),
                x => x.NextQuestRewardUpgradeUnlock++);
        }

        public Upgrade GetNextWorkerProductionUpgrade(Player player)
        {
            Int32 currentUpgradePercent = (Int32)(Calculator.GetWorkerPointMultiplier(player.NextWorkerProductionUpgradeUnlock) * 100);
            Int32 nextUpgradePercent = (Int32)(Calculator.GetWorkerPointMultiplier(player.NextWorkerProductionUpgradeUnlock.Next()) * 100);
            return new Upgrade(
                String.Format(ProductionDescription, currentUpgradePercent, nextUpgradePercent),
                player.NextWorkerProductionUpgradeUnlock,
                (Int32)(100 + Math.Pow(2, (Int32)player.NextWorkerProductionUpgradeUnlock) * 100),
                x => x.NextWorkerProductionUpgradeUnlock++);
        }

        public Upgrade GetNextUpgrade(Player player)
        {
            if (player.NextCriticalCheeseUpgradeUnlock > player.NextStorageUpgradeUnlock)
            {
                return GetNextStorageUpgrade(player);
            }
            else if (player.NextQuestRewardUpgradeUnlock > player.NextCriticalCheeseUpgradeUnlock)
            {
                return GetNextCriticalCheeseUpgrade(player);
            }
            else if (player.NextWorkerProductionUpgradeUnlock > player.NextQuestRewardUpgradeUnlock)
            {
                return GetNextQuestRewardUpgrade(player);
            }
            else if (player.NextWorkerProductionUpgradeUnlock <= Rank.Legend)
            {
                return GetNextWorkerProductionUpgrade(player);
            }
            else
            {
                return null;
            }
        }
    }
}
