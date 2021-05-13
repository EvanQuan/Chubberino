using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Utility;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public static class ListQuestRepositoryExtensions
    {
        public static Boolean TryGetNextToUnlock(
            this IReadOnlyList<Quest> repository,
            Player player,
            out Quest quest)
        {
            return repository.TryGet(player.QuestsUnlockedCount, out quest);
        }
    }
}
