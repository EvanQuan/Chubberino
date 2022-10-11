using System.Collections.Generic;
using Chubberino.Common.Extensions;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Quests;

public static class ListQuestRepositoryExtensions
{
    public static Option<Quest> TryGetNextToUnlock(
        this IReadOnlyList<Quest> repository,
        Player player)
        => repository.TryGet(player.QuestsUnlockedCount);
}
