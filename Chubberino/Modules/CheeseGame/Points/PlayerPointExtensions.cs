using Chubberino.Modules.CheeseGame.Hazards;
using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Items.Workers;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Ranks;
using Chubberino.Utility;
using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public static class PlayerPointExtensions
    {

        /// <summary>
        /// Point multiplier for critical cheese.
        /// </summary>
        public const Int32 CriticalCheeseMultiplier = 5;

        /// <summary>
        /// Add or substract points from the player. Does not save the database.
        /// Ensures that points cannot exceed <see cref="PlayerExtensions.PlayerInformationExtensions.GetTotalStorage(Player)"/> or go below 0.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="points"></param>
        public static void AddPoints(this Player player, Double points)
        {
            player.AddPoints((Int32) points);
        }

        /// <summary>
        /// Add or substract points from the player. Does not save the database.
        /// Ensures that points cannot exceed <see cref="PlayerExtensions.PlayerInformationExtensions.GetTotalStorage(Player)"/> or go below 0.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="points"></param>
        public static void AddPoints(this Player player, Int32 points)
        {
            player.Points = (player.Points + points)
                .Min(player.GetTotalStorage())
                .Max(0);
        }

        /// <summary>
        /// Modify <paramref name="points"/> by the specified <paramref name="player"/>'s worker and prestige bonus.
        /// </summary>
        /// <param name="player">Player to get bonuses from.</param>
        /// <param name="points">Initial points to modify.</param>
        /// <param name="isCritical">Indicates if a critical bonus should be applied.</param>
        /// <returns></returns>
        public static Int32 GetModifiedPoints(this Player player, Int32 points, Boolean isCritical = false)
        {
            // Cannot reach negative points.
            // Cannot go above the point storage.
            // Prestige bonus is only applied to base cheese gained.
            // Workers will collectively add at least 1.
            Int32 workerPoints = 0;
            if (!player.IsInfested())
            {
                Double workerPointMultipler = player.GetWorkerPointMultiplier();
                Int32 absoluteWorkerPoints = (Int32)(Math.Abs(points) * (player.WorkerCount * workerPointMultipler)).Max(player.WorkerCount == 0 ? 0 : 1);
                workerPoints = Math.Sign(points) * absoluteWorkerPoints;
            }

            Int32 pointsToAddRaw = (Int32)(points * (1 + RankManager.PrestigeBonus * player.Prestige) + workerPoints);

            return pointsToAddRaw * (isCritical ? CriticalCheeseMultiplier : 1);
        }
    }
}
