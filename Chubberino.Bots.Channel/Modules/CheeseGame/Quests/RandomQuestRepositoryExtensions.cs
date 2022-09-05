using System;
using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Common.Extensions;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Quests;

public static class RandomQuestRepositoryExtensions
{
    public static Quest NextElement(this Random random, IQuestRepository questRepository, Player player)
    {
        return random.TryPercentChance(player.GetRareQuestChance())
            ? random.NextElement(questRepository.RareQuests)
            : random.NextElement(questRepository.CommonQuests, player.QuestsUnlockedCount - 1);
    }
}
