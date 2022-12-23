using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Recipes;
using Chubberino.Bots.Channel.Modules.CheeseGame.Points;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Upgrades.RecipeModifiers;

public static class RecipeModifierExtensions
{
    public static RecipeInfo Modify(this in RecipeModifier modifier, RecipeInfo cheeseType)
        => new(modifier.Name + " " + cheeseType.Name, (Math.Sign(cheeseType.Points) * modifier.Points) + cheeseType.Points);

    public static Int32 GetPoints(this in RecipeModifier modifier, Player player)
        => player.GetModifiedPoints(modifier.Points);
}
