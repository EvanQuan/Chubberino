using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Utility;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Items.Upgrades.RecipeModifiers
{
    public static class ListRecipeModifierRepositoryExtensions
    {
        public static Boolean TryGetNextToUnlock(
            this IReadOnlyList<RecipeModifier> repository,
            Player player,
            out RecipeModifier cheeseModifier)
        {
            // The default modifier is null, the 0th element of the repository,
            // which is always a possibility.
            // We add 1 to ensure this can be picked.
            return repository.TryGet((Int32)(player.NextCheeseModifierUpgradeUnlock + 1), out cheeseModifier);
        }
    }
}
