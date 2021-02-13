using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Hazards
{
    public interface IHazardManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns>true if the <paramref name="player"/> <see cref="Player.IsMouseInfested"/> status has changed. </returns>
        Boolean ResolveStartMouseInfestation(Player player);

        Int32 GetMouseInfestationPointLoss(Int32 points);
    }
}
