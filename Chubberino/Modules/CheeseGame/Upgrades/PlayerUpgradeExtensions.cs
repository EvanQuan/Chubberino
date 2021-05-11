using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Rankings;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public static class PlayerUpgradeExtensions
    {
        public static UpgradeType GetLastUpgradeUnlocked(this Player player)
        {
            if (player.NextWorkerProductionUpgradeUnlock > player.NextStorageUpgradeUnlock)
            {
                return UpgradeType.WorkerProduction;
            }
            else if (player.NextQuestRewardUpgradeUnlock > player.NextWorkerProductionUpgradeUnlock)
            {
                return UpgradeType.Quest;
            }
            else if (player.NextCriticalCheeseUpgradeUnlock > player.NextQuestRewardUpgradeUnlock)
            {
                return UpgradeType.CriticalCheese;
            }
            else if (player.NextCheeseModifierUpgradeUnlock > player.NextCriticalCheeseUpgradeUnlock)
            {
                return UpgradeType.CheeseModifier;
            }
            else if (player.NextCheeseModifierUpgradeUnlock <= Rank.Legend)
            {
                // Either no unlocks have been bought
                if (player.NextStorageUpgradeUnlock == Rank.Bronze)
                {
                    return UpgradeType.None;
                }
                // or at the start of a new rank's upgrades.
                return UpgradeType.Storage;
            }
            else
            {
                return UpgradeType.Storage;
            }
        }

        public static UpgradeType GetNextUpgradeToUnlock(this Player player)
        {
            if (player.NextWorkerProductionUpgradeUnlock > player.NextStorageUpgradeUnlock)
            {
                return UpgradeType.Storage;
            }
            else if (player.NextQuestRewardUpgradeUnlock > player.NextWorkerProductionUpgradeUnlock)
            {
                return UpgradeType.WorkerProduction;
            }
            else if (player.NextCriticalCheeseUpgradeUnlock > player.NextQuestRewardUpgradeUnlock)
            {
                return UpgradeType.Quest;
            }
            else if (player.NextCheeseModifierUpgradeUnlock > player.NextCriticalCheeseUpgradeUnlock)
            {
                return UpgradeType.CriticalCheese;
            }
            else if (player.NextCheeseModifierUpgradeUnlock <= Rank.Legend)
            {
                return UpgradeType.CheeseModifier;
            }
            else
            {
                return UpgradeType.None;
            }
        }
    }
}
