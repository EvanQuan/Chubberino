using Chubberino.Database.Models;
using System;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Recipes;

public static class PlayerRecipeExtensions
{
    public static Boolean HasUnlockedAllRecipes(this Player player)
    {
        return player.CheeseUnlocked + 1 >= RecipeRepository.Recipes.Count;
    }
}
