using System;
using System.Collections.Generic;
using Chubberino.Common.Extensions;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Quests;

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
