using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public static class PlayerUpgradeExtensions
    {
        public static UpgradeType GetPreviousUpgradeUnlocked(this Player player)
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

        public static Upgrade GetNextCheeseModifierUpgrade(this Player player)
        {
            return player.NextCheeseModifierUpgradeUnlock.GetCheeseModifierUpgrade();
        }

        public static Upgrade GetNextCriticalCheeseUpgrade(this Player player)
        {
            return player.NextCriticalCheeseUpgradeUnlock.GetCriticalCheeseUpgrade();
        }

        public static Upgrade GetNextQuestRewardUpgrade(this Player player)
        {
            return player.NextQuestRewardUpgradeUnlock.GetQuestUpgrade();
        }

        public static Upgrade GetNextWorkerProductionUpgrade(this Player player)
        {
            return player.NextWorkerProductionUpgradeUnlock.GetWorkerProductionUpgrade();
        }

        public static Upgrade GetNextStorageUpgrade(this Player player)
        {
            return player.NextStorageUpgradeUnlock.GetStorageUpgrade();
        }

        public static Upgrade GetPreviousCheeseModifierUpgrade(this Player player)
        {
            return (player.NextCheeseModifierUpgradeUnlock - 1).GetCheeseModifierUpgrade();
        }

        public static Upgrade GetPreviousCriticalCheeseUpgrade(this Player player)
        {
            return (player.NextCriticalCheeseUpgradeUnlock - 1).GetCriticalCheeseUpgrade();
        }

        public static Upgrade GetPreviousQuestRewardUpgrade(this Player player)
        {
            return (player.NextQuestRewardUpgradeUnlock - 1).GetQuestUpgrade();
        }

        public static Upgrade GetPreviousWorkerProductionUpgrade(this Player player)
        {
            return (player.NextWorkerProductionUpgradeUnlock - 1).GetWorkerProductionUpgrade();
        }

        public static Upgrade GetPreviousStorageUpgrade(this Player player)
        {
            return (player.NextStorageUpgradeUnlock - 1).GetStorageUpgrade();
        }

        private static Boolean TryGetTypeToNextUpgrade(UpgradeType type, out Func<Player, Upgrade> upgrade)
        {
            upgrade = (type) switch
            {
                UpgradeType.CheeseModifier => GetNextCheeseModifierUpgrade,
                UpgradeType.CriticalCheese => GetNextCriticalCheeseUpgrade,
                UpgradeType.Quest => GetNextQuestRewardUpgrade,
                UpgradeType.WorkerProduction => GetNextWorkerProductionUpgrade,
                UpgradeType.Storage => GetNextStorageUpgrade,
                _ => default,
            };

            return upgrade != default;
        }
        
        private static Boolean TryGetTypeToPreviousUpgrade(UpgradeType type, out Func<Player, Upgrade> upgrade)
        {
            upgrade = (type) switch
            {
                UpgradeType.CheeseModifier => GetPreviousCheeseModifierUpgrade,
                UpgradeType.CriticalCheese => GetPreviousCriticalCheeseUpgrade,
                UpgradeType.Quest => GetPreviousQuestRewardUpgrade,
                UpgradeType.WorkerProduction => GetPreviousWorkerProductionUpgrade,
                UpgradeType.Storage => GetPreviousStorageUpgrade,
                _ => default,
            };

            return upgrade != default;
        }

        public static Boolean TryPreviousUpgradeUnlocked(this Player player, out Upgrade upgrade)
        {
            if (TryGetTypeToPreviousUpgrade(player.GetPreviousUpgradeUnlocked(), out var playerToUpgrade))
            {
                upgrade = playerToUpgrade(player);
                return true;
            }
            upgrade = default;
            return false;
        }


        public static Boolean TryGetNextUpgradeToUnlock(this Player player, out Upgrade upgrade)
        {
            if (TryGetTypeToNextUpgrade(player.GetNextUpgradeToUnlock(), out var playerToUpgrade))
            {
                upgrade = playerToUpgrade(player);
                return true;
            }
            upgrade = default;
            return false;
        }

    }
}
