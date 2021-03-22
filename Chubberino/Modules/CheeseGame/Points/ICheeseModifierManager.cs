using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public interface ICheeseModifierManager
    {
        CheeseModifier GetRandomModifier(Rank modifierRank);

        Boolean TryGetNextModifierToUnlock(Rank modifierRank, out CheeseModifier cheeseModifier);
    }
}
