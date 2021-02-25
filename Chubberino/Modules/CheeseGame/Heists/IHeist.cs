using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public interface IHeist
    {
        /// <summary>
        /// Player that initiated the heist.
        /// </summary>
        String InitiatorName { get; }

        /// <summary>
        /// Start the heist.
        /// </summary>
        /// <returns>true if the heist started; otherwise, false.</returns>
        Boolean Start();

        /// <summary>
        /// Add or update a wager for the heist.
        /// </summary>
        /// <param name="player">Player to add a wager for.</param>
        /// <param name="points">Points wagered by <paramref name="player"/>.</param>
        public void UpdateWager(Player player, Int32 points);
    }
}