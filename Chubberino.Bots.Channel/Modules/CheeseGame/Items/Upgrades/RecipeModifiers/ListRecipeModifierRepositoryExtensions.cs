using Chubberino.Common.Extensions;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Upgrades.RecipeModifiers;

public static class ListRecipeModifierRepositoryExtensions
{
    public static Option<RecipeModifier> TryGetNextToUnlock(
        this Option<RecipeModifier>[] repository,
        Player player) =>
        // The default modifier is None, the 0th element of the repository,
        // which is always a possibility.
        // We add 1 to ensure this can be picked.
        repository
            .TryGet((Int32)(player.NextCheeseModifierUpgradeUnlock + 1))
            .Bind(modifier => modifier);
}
