using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Items.Upgrades
{
    public static class PlayerUpgradeExtensions
    {
        public static UpgradeType GetPreviousUpgradeUnlocked(this Player player)
        {
            if (player.NextWorkerProductionUpgradeUnlock > player.NextStorageUpgradeUnlock)
            {
                return UpgradeType.WorkerProduction;
            }
            else if (player.NextQuestUpgradeUnlock > player.NextWorkerProductionUpgradeUnlock)
            {
                return UpgradeType.Quest;
            }
            else if (player.NextCriticalCheeseUpgradeUnlock > player.NextQuestUpgradeUnlock)
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
            else if (player.NextQuestUpgradeUnlock > player.NextWorkerProductionUpgradeUnlock)
            {
                return UpgradeType.WorkerProduction;
            }
            else if (player.NextCriticalCheeseUpgradeUnlock > player.NextQuestUpgradeUnlock)
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

        public static UpgradeInfo GetNextCheeseModifierUpgrade(this Player player)
        {
            return player.NextCheeseModifierUpgradeUnlock.GetCheeseModifierUpgrade();
        }

        public static UpgradeInfo GetNextCriticalCheeseUpgrade(this Player player)
        {
            return player.NextCriticalCheeseUpgradeUnlock.GetCriticalCheeseUpgrade();
        }

        public static UpgradeInfo GetNextQuestUpgrade(this Player player)
        {
            return player.NextQuestUpgradeUnlock.GetQuestUpgrade();
        }

        public static UpgradeInfo GetNextWorkerProductionUpgrade(this Player player)
        {
            return player.NextWorkerProductionUpgradeUnlock.GetWorkerProductionUpgrade();
        }

        public static UpgradeInfo GetNextStorageUpgrade(this Player player)
        {
            return player.NextStorageUpgradeUnlock.GetStorageUpgrade();
        }

        public static UpgradeInfo GetPreviousCheeseModifierUpgrade(this Player player)
        {
            return (player.NextCheeseModifierUpgradeUnlock - 1).GetCheeseModifierUpgrade();
        }

        public static UpgradeInfo GetPreviousCriticalCheeseUpgrade(this Player player)
        {
            return (player.NextCriticalCheeseUpgradeUnlock - 1).GetCriticalCheeseUpgrade();
        }

        public static UpgradeInfo GetPreviousQuestUpgrade(this Player player)
        {
            return (player.NextQuestUpgradeUnlock - 1).GetQuestUpgrade();
        }

        public static UpgradeInfo GetPreviousWorkerProductionUpgrade(this Player player)
        {
            return (player.NextWorkerProductionUpgradeUnlock - 1).GetWorkerProductionUpgrade();
        }

        public static UpgradeInfo GetPreviousStorageUpgrade(this Player player)
        {
            return (player.NextStorageUpgradeUnlock - 1).GetStorageUpgrade();
        }

        private static Boolean TryGetTypeToNextUpgrade(UpgradeType type, out Func<Player, UpgradeInfo> upgrade)
        {
            upgrade = (type) switch
            {
                UpgradeType.CheeseModifier => GetNextCheeseModifierUpgrade,
                UpgradeType.CriticalCheese => GetNextCriticalCheeseUpgrade,
                UpgradeType.Quest => GetNextQuestUpgrade,
                UpgradeType.WorkerProduction => GetNextWorkerProductionUpgrade,
                UpgradeType.Storage => GetNextStorageUpgrade,
                _ => default,
            };

            return upgrade != default;
        }
        
        private static Boolean TryGetTypeToPreviousUpgrade(UpgradeType type, out Func<Player, UpgradeInfo> upgrade)
        {
            upgrade = (type) switch
            {
                UpgradeType.CheeseModifier => GetPreviousCheeseModifierUpgrade,
                UpgradeType.CriticalCheese => GetPreviousCriticalCheeseUpgrade,
                UpgradeType.Quest => GetPreviousQuestUpgrade,
                UpgradeType.WorkerProduction => GetPreviousWorkerProductionUpgrade,
                UpgradeType.Storage => GetPreviousStorageUpgrade,
                _ => default,
            };

            return upgrade != default;
        }

        public static Boolean TryPreviousUpgradeUnlocked(this Player player, out UpgradeInfo upgrade)
        {
            if (TryGetTypeToPreviousUpgrade(player.GetPreviousUpgradeUnlocked(), out var playerToUpgrade))
            {
                upgrade = playerToUpgrade(player);
                return true;
            }
            upgrade = default;
            return false;
        }


        public static Boolean TryGetNextUpgradeToUnlock(this Player player, out UpgradeInfo upgrade)
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
