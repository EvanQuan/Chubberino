using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Utility;
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
            player.NextQuestRewardUpgradeUnlock = 0;
            player.NextStorageUpgradeUnlock = 0;
            player.NextCriticalCheeseUpgradeUnlock = 0;
            player.MouseTrapCount = 0;
            player.MouseCount = 0;

            return player;
        }

        public static Boolean IsMouseInfested(this Player player)
        {
            return player.MouseCount > 0;
        }

        public static String GetDisplayName(this Player player)
        {
            String prestige = player.Prestige > 0 ? "P" + player.Prestige + " " : String.Empty;
            String cheese = $"{player.Points}/{player.GetTotalStorage()} cheese";
            String workers = $"{player.WorkerCount}/{player.PopulationCount} workers";
            String mousetraps = $"{player.MouseTrapCount} mousetrap{(player.MouseTrapCount != 1 ? "s" : String.Empty)}";
            return $"{player.Name} [{prestige}{player.Rank}, {cheese}, {workers}, {mousetraps}]";
        }

        /// <summary>
        /// Add or substract points from the player. Does not save the database.
        /// Ensures that points cannot exceed <see cref="GetTotalStorage(Player)"/> or go below 0.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="points"></param>
        public static void AddPoints(this Player player, Double points)
        {
            player.AddPoints((Int32) points);
        }

        public static void AddPoints(this Player player, CheeseType cheese, ICalculator calculator, Boolean withWorkers = true, Boolean isCritical = false)
        {
            // Cannot reach negative points.
            // Cannot go above the point storage.
            // Prestige bonus is only applied to base cheese gained.
            // Workers will collectively add at least 1.
            Int32 workerPoints = 0;
            if (withWorkers)
            {
                Double workerPointMultipler = calculator.GetWorkerPointMultiplier(player.NextWorkerProductionUpgradeUnlock);
                Int32 absoluteWorkerPoints = (Int32)(Math.Abs(cheese.PointValue) * (player.WorkerCount * workerPointMultipler)).Max(player.WorkerCount == 0 ? 0 : 1);
                workerPoints = Math.Sign(cheese.PointValue) * absoluteWorkerPoints;
            }

            Double pointsToAddRaw = cheese.PointValue * (1 + Constants.PrestigeBonus * player.Prestige) + workerPoints;

            Double pointsToAddWithCritical = pointsToAddRaw * (isCritical ? Constants.CriticalCheeseMultiplier : 1);

            Double newPointsRaw = player.Points + pointsToAddWithCritical;

            Int32 newPointsBounded = (Int32)newPointsRaw
                .Min(player.GetTotalStorage())
                .Max(0);

            player.Points = newPointsBounded;
        }

        /// <summary>
        /// Add or substract points from the player. Does not save the database.
        /// Ensures that points cannot exceed <see cref="GetTotalStorage(Player)"/> or go below 0.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="points"></param>
        public static void AddPoints(this Player player, Int32 points)
        {
            player.Points = (player.Points + points)
                .Min(player.GetTotalStorage())
                .Max(0);
        }

        public static Double GetStorageUpgradeMultiplier(this Player player)
        {
            return 1 + (Int32)player.NextStorageUpgradeUnlock * Constants.StorageUpgradePercent;
        }

        public static Int32 GetTotalStorage(this Player player)
        {
            return (Int32)(player.MaximumPointStorage * player.GetStorageUpgradeMultiplier());
        }

        public static Double GetQuestSuccessChance(this Player player)
        {
            Double baseSuccessChance = Constants.QuestBaseSuccessChance;

            Double workerSuccessChance = player.IsMouseInfested() ? 0 : player.WorkerCount * Constants.QuestWorkerSuccessPercent;

            return baseSuccessChance + workerSuccessChance;
        }

        public static Double GetQuestRewardMultiplier(this Player player)
        {
            return 1 + ((Int32)player.NextQuestRewardUpgradeUnlock * Constants.QuestRewardUpgradePercent);
        }
    }
}
