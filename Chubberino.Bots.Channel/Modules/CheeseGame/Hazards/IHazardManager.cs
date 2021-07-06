using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Hazards
{
    public interface IHazardManager
    {
        /// <summary>
        /// Update the infestation status of the <paramref name="player"/>.
        /// </summary>
        /// <param name="player">Player to potentially modify to reflect new infestation status.</param>
        /// <returns>The infestation message after update.</returns>
        String UpdateInfestationStatus(Player player);
    }
}
