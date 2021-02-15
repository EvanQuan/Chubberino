using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Hazards
{
    public interface IHazardManager
    {
        /// <summary>
        /// Update the mouse infestation status of the <paramref name="player"/>.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The infestation message after update. </returns>
        String UpdateMouseInfestationStatus(Player player);

        Int32 GetMouseInfestationPointLoss(Int32 points);
    }
}
