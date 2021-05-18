using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public static class PlayerQuestExtensions
    {
        public static Double GetRareQuestChance(this Player player)
        {
            return player.NextQuestUpgradeUnlock.GetRareQuestChance();
        }
    }
}
