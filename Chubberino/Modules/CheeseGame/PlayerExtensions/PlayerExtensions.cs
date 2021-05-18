using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.PlayerExtensions
{
    public static class PlayerExtensions
    {
        public static Player ResetRank(this Player player)
        {
            player.MaximumPointStorage = 50;
            player.Points = 0;
            player.PopulationCount = 0;
            player.WorkerCount = 0;
            player.Rank = Rankings.Rank.Bronze;
            player.CheeseUnlocked = 0;
            player.NextWorkerProductionUpgradeUnlock = 0;
            player.NextQuestUpgradeUnlock = 0;
            player.NextCheeseModifierUpgradeUnlock = 0;
            player.NextStorageUpgradeUnlock = 0;
            player.NextCriticalCheeseUpgradeUnlock = 0;
            player.MouseTrapCount = 0;
            player.RatCount = 0;
            player.CatCount = 0;
            player.ContributedDamageToBoss = 0;
            player.QuestsUnlockedCount = 0;
            player.GearCount = 0;

            return player;
        }

        public static String GetDisplayName(this Player player)
        {
            String prestige = player.Prestige > 0 ? "P" + player.Prestige + " " : String.Empty;
            String cheese = $"{player.Points}/{player.GetTotalStorage()} cheese";
            String workers = $"{player.WorkerCount}/{player.PopulationCount} workers";
            String mousetraps = $"{player.MouseTrapCount} mousetrap{(player.MouseTrapCount != 1 ? "s" : String.Empty)}";
            return $"{player.Name} [{prestige}{player.Rank}, {cheese}, {workers}, {mousetraps}]";
        }



        public static Double GetStorageUpgradeMultiplier(this Player player)
        {
            return 1 + (Int32)player.NextStorageUpgradeUnlock * Constants.StorageUpgradePercent;
        }

        public static Int32 GetTotalStorage(this Player player)
        {
            return (Int32)(player.MaximumPointStorage * player.GetStorageUpgradeMultiplier());
        }

        public static Boolean HasUnlockedAllCheeses(this Player player)
        {
            return player.CheeseUnlocked + 1 >= CheeseRepository.Cheeses.Count;
        }
    }
}
