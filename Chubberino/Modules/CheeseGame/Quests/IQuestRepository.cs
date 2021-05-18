using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public interface IQuestRepository
    {
        public Double RareQuestChance { get; }

        IReadOnlyList<Quest> CommonQuests { get; }

        IReadOnlyList<Quest> RareQuests { get; }
    }
}
