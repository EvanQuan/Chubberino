using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Utility;
using System;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public static class RandomQuestRepositoryExtensions
    {
        public static Quest NextElement(this Random random, IQuestRepository questRepository, Player player)
        {
            return random.TryPercentChance(player.GetRareQuestChance())
                ? random.NextElement(questRepository.RareQuests)
                : random.NextElement(questRepository.CommonQuests, player.QuestsUnlockedCount - 1);
        }
    }
}
