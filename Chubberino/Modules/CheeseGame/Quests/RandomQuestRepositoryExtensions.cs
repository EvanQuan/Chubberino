using Chubberino.Utility;
using System;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public static class RandomQuestRepositoryExtensions
    {
        public static Quest NextElement(this Random random, IQuestRepository questRepository, Int32 unlockedCount)
        {
            return random.TryPercentChance(questRepository.RareQuestChance)
                ? random.NextElement(questRepository.RareQuests)
                : random.NextElement(questRepository.CommonQuests, unlockedCount);
        }
    }
}
