using Chubberino.Modules.CheeseGame.Models;
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
            player.MouseTrapCount = 0;
            player.IsMouseInfested = false;

            return player;
        }

        public static String GetDisplayName(this Player player)
        {
            return $"{player.Name} [P{player.Prestige} {player.Rank}, {player.Points}/{player.MaximumPointStorage} cheese, {player.WorkerCount}/{player.PopulationCount} workers, {player.MouseTrapCount} mousetraps]";
        }

        /// <summary>
        /// Add or substract points from the player. Does not save the database.
        /// Ensures that points cannot exceed <see cref="Player.MaximumPointStorage"/> or go below 0.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="points"></param>
        public static void AddPoints(this Player player, Double points)
        {
            player.AddPoints((Int32) points);
        }

        /// <summary>
        /// Add or substract points from the player. Does not save the database.
        /// Ensures that points cannot exceed <see cref="Player.MaximumPointStorage"/> or go below 0.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="points"></param>
        public static void AddPoints(this Player player, Int32 points)
        {
            player.Points = Math.Max(Math.Min(player.Points + points, player.MaximumPointStorage), 0);
        }
    }
}
