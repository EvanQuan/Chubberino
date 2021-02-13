using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public sealed class UpgradeManager : IUpgradeManager
    {
        private const String QuestHelpDescription = "+{0}% quest success chance";

        private const String ProductionDescription = "+{0} cheese";

        public Upgrade GetNextUpgradeToUnlock(Player player)
        {
            if (player.LastWorkerProductionUpgradeUnlocked > player.LastWorkerQuestHelpUnlocked)
            {
                return new Upgrade(
                    String.Format(QuestHelpDescription, (Int32)(player.LastWorkerQuestHelpUnlocked + 1)),
                    player.LastWorkerQuestHelpUnlocked,
                    50 + 100 * (Int32)player.LastWorkerQuestHelpUnlocked,
                    x => x.LastWorkerQuestHelpUnlocked++);
            }
            else if (player.LastWorkerProductionUpgradeUnlocked == Rankings.Rank.Legend)
            {
                return null;
            }
            else
            {
                return new Upgrade(
                    String.Format(ProductionDescription, (Int32)(player.LastWorkerProductionUpgradeUnlocked + 1)),
                    player.LastWorkerProductionUpgradeUnlocked,
                    100 + 200 * (Int32)player.LastWorkerProductionUpgradeUnlocked,
                    x => x.LastWorkerProductionUpgradeUnlocked++);
            }
        }
    }
}
