using Chubberino.Modules.CheeseGame.Hazards;
using Chubberino.Modules.CheeseGame.Items;
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

        public static Boolean HasQuestingUnlocked(this Player player)
        {
            return player.QuestsUnlockedCount > 0;
        }

        public static Double GetQuestSuccessChance(this Player player)
        {
            Double baseSuccessChance = Constants.QuestBaseSuccessChance;

            Double workerSuccessChance = player.IsInfested() ? 0 : player.GearCount * Gear.QuestSuccessBonus;

            return baseSuccessChance + workerSuccessChance;
        }
    }
}
