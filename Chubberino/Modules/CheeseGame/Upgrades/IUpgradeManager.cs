using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public interface IUpgradeManager
    {
        /// <summary>
        /// Get the next upgrade for the player to unlock.
        /// </summary>
        /// <param name="player">Player to check for upgrade to unlock.</param>
        /// <param name="upgrade">Upgrade to unlock, if any; null otherwise.</param>
        /// <returns>The next upgrade for the <paramref name="player"/> to unlocked; or null if no more upgrades available.</returns>
        Boolean TryGetNextUpgradeToUnlock(Player player, out Upgrade upgrade);
    }
}
