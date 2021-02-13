using Chubberino.Modules.CheeseGame.Models;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public interface IUpgradeManager
    {
        /// <summary>
        /// Get the next upgrade for the player to unlock.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The next upgrade for the <paramref name="player"/> to unlocked; or null if no more upgrades available.</returns>
        Upgrade GetNextUpgradeToUnlock(Player player);
    }
}
