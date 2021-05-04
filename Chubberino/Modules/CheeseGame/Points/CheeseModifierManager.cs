using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Utility;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class CheeseModifierManager : ICheeseModifierManager
    {
        public static IReadOnlyList<CheeseModifier> Modifiers { get; } = new List<CheeseModifier>()
        {
            null,
            new CheeseModifier("fresh", 1),
            new CheeseModifier("sharp", 2),
            new CheeseModifier("extra-salted", 3),
            new CheeseModifier("smoked", 5),
            new CheeseModifier("extra-creamy", 7),
            new CheeseModifier("aged", 9),
            new CheeseModifier("extra-aged", 12),
            new CheeseModifier("perfect", 15),
        };

        public Random Random { get; }

        public CheeseModifierManager(Random random)
        {
            Random = random;
        }

        public CheeseModifier GetRandomModifier(Rank modifierUnlocked)
        {
            return Modifiers[Random.Next(((Int32)modifierUnlocked).Min((Int32)Rank.None).Max((Int32)Rank.Bronze))];
        }

        public Boolean TryGetNextModifierToUnlock(Rank modifierRank, out CheeseModifier cheeseModifier)
        {
            Int32 nextModifierIndex = (Int32)(modifierRank + 1);

            if (nextModifierIndex > Modifiers.Count - 1)
            {
                cheeseModifier = default;
                return false;
            }

            cheeseModifier = Modifiers[nextModifierIndex];
            return true;
        }
    }
}
