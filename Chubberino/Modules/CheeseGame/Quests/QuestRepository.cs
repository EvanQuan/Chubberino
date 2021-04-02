using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Utility;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class QuestRepository : IRepository<Quest>
    {
        public static IReadOnlyList<Quest> Quests { get; } = new List<Quest>()
        {

        };
        public Random Random { get; }

        public QuestRepository(Random random)
        {
            Random = random;
        }

        public Quest GetRandom(Int32 unlocked)
        {
            return Quests[Random.Next(unlocked.Min(Quests.Count - 1).Max(0))];
        }

        public Boolean TryGetNextToUnlock(Player player, out Quest nextUnlock)
        {
            throw new NotImplementedException();
        }
    }
}
