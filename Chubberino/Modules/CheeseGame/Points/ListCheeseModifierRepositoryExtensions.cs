using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Utility;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Points
{
    public static class ListCheeseModifierRepositoryExtensions
    {
        public static Boolean TryGetNextToUnlock(
            this IReadOnlyList<CheeseModifier> repository,
            Player player,
            out CheeseModifier cheeseModifier)
        {
            // The default modifier is null, the 0th element of the repository,
            // which is always a possibility.
            // We add 1 to ensure this can be picked.
            return repository.TryGet((Int32)(player.NextCheeseModifierUpgradeUnlock + 1), out cheeseModifier);
        }
    }
}
