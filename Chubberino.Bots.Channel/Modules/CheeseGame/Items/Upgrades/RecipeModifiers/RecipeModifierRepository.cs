using System.Collections.Generic;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Upgrades.RecipeModifiers;

public static class RecipeModifierRepository
{
    public static Option<RecipeModifier>[] Modifiers { get; } = new Option<RecipeModifier>[]
    {
        Option<RecipeModifier>.None,
        new RecipeModifier("fresh", 1),
        new RecipeModifier("sharp", 2),
        new RecipeModifier("extra-salted", 3),
        new RecipeModifier("smoked", 5),
        new RecipeModifier("extra-creamy", 7),
        new RecipeModifier("aged", 9),
        new RecipeModifier("extra-aged", 12),
        new RecipeModifier("perfect", 15),
    };
}
