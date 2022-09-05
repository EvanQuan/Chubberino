using System.Collections.Generic;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Quests;

public interface IQuestRepository
{
    IReadOnlyList<Quest> CommonQuests { get; }

    IReadOnlyList<Quest> RareQuests { get; }
}
