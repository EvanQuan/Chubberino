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
            player.LastWorkerProductionUpgradeUnlocked = 0;
            player.LastWorkerQuestHelpUnlocked = 0;
            player.LastStorageUpgradeUnlocked = 0;
            player.MouseTrapCount = 0;
            player.IsMouseInfested = false;

            return player;
        }

        public static String GetDisplayName(this Player player)
        {
            return $"{player.Name} [P{player.Prestige} {player.Rank}, {player.Points}/{player.GetTotalStorage()} cheese, {player.WorkerCount}/{player.PopulationCount} workers, {player.MouseTrapCount} mousetraps]";
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

        public static void AddPoints(this Player player, CheeseType cheese, Boolean withWorkers = true)
        {
            // Cannot reach negative points.
            // Cannot go above the point storage.
            // Prestige bonus is only applied to base cheese gained.
            // Workers will collectively add at least 1.
            Int32 workerPoints = 0;
            if (withWorkers)
            {
                Double workerPointMultipler = ((Int32)player.LastWorkerProductionUpgradeUnlocked + 1) * Constants.WorkerUpgradePercent;
                Int32 absoluteWorkerPoints = (Int32)Math.Max(Math.Abs(cheese.PointValue) * (player.WorkerCount * workerPointMultipler), player.WorkerCount == 0 ? 0 : 1);
                workerPoints = Math.Sign(cheese.PointValue) * absoluteWorkerPoints;
            }

            var newPoints = (Int32)Math.Min(Math.Max(player.Points + (cheese.PointValue * (1 + Constants.PrestigeBonus * player.Prestige)) + workerPoints, 0), player.GetTotalStorage());
            player.Points = newPoints;
        }

        /// <summary>
        /// Add or substract points from the player. Does not save the database.
        /// Ensures that points cannot exceed <see cref="PlayerExtensions.GetTotalStorage(Player)"/> or go below 0.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="points"></param>
        public static void AddPoints(this Player player, Int32 points)
        {
            player.Points = Math.Max(Math.Min(player.Points + points, player.GetTotalStorage()), 0);
        }

        public static Int32 GetTotalStorage(this Player player)
        {
            return (Int32)(player.MaximumPointStorage * (1 + (Int32)player.LastStorageUpgradeUnlocked * Constants.StorageUpgradePercent));
        }

        public static Double GetQuestSuccessChance(this Player player)
        {
            return Constants.QuestBaseSuccessChance * (1 + player.WorkerCount * (Constants.QuestWorkerSuccessBonus * ((Int32)player.LastWorkerQuestHelpUnlocked + 1)));
        }
    }
}
