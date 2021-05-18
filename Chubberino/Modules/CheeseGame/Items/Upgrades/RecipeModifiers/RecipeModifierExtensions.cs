using Chubberino.Modules.CheeseGame.Items.Recipes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Items.Upgrades.RecipeModifiers
{
    public static class RecipeModifierExtensions
    {
        public static RecipeInfo Modify(this RecipeModifier modifier, RecipeInfo cheeseType)
        {
            if (modifier == null) { return cheeseType; }
            return new RecipeInfo(modifier.Name + " " + cheeseType.Name, Math.Sign(cheeseType.Points) * modifier.Points + cheeseType.Points);
        }

        public static Int32 GetPoints(this RecipeModifier modifier, Player player)
        {
            return player.GetModifiedPoints(modifier.Points);
        }
    }
}
