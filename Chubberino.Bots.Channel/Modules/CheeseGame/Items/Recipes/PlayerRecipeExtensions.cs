using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Recipes;

public static class PlayerRecipeExtensions
{
    public static Boolean HasUnlockedAllRecipes(this Player player)
        => player.CheeseUnlocked + 1 >= RecipeRepository.Recipes.Count;
}
