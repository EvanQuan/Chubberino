using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Models;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public interface IHeist
    {
        /// <summary>
        /// Wagers in the heist.
        /// </summary>
        public IList<Wager> Wagers { get; }

        /// <summary>
        /// Start the heist.
        /// </summary>
        /// <returns>true if the heist started; otherwise, false.</returns>
        Boolean Start(IApplicationContext context);

        /// <summary>
        /// Add or update a wager for the heist.
        /// </summary>
        /// <param name="player">Player to add a wager for.</param>
        /// <param name="points">Points wagered by <paramref name="player"/>.</param>
        /// <param name="silent">Indicates if the update should be silent.</param>
        public void UpdateWager(IApplicationContext context, Player player, Func<Player, Int32> points, Boolean silent = false);
    }
}