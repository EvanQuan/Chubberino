using Chubberino.Modules.CheeseGame.Models;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public sealed class UpgradeManager : IUpgradeManager
    {
        public Upgrade GetNextUpgradeToUnlock(Player player)
        {
            return player.GetNextUpgrade();
        }
    }
}
