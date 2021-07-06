using Chubberino.Modules.CheeseGame.Items.Recipes;
using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Items.Upgrades.Recipes
{
    public static class PlayerRecipeExtensions
    {
        public static Boolean HasUnlockedAllRecipes(this Player player)
        {
            return player.CheeseUnlocked + 1 >= RecipeRepository.Recipes.Count;
        }
    }
}
