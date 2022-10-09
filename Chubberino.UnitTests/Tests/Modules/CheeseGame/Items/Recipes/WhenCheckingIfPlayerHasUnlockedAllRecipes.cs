using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Recipes;
using Chubberino.Database.Models;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items.Recipes;

public sealed class WhenCheckingIfPlayerHasUnlockedAllRecipes
{
    private Player Player { get; }

    public WhenCheckingIfPlayerHasUnlockedAllRecipes()
    {
        Player = new();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void ShouldReturnFalse(Int32 recipesUnlocked)
    {
        Player.CheeseUnlocked = recipesUnlocked;

        var result = Player.HasUnlockedAllRecipes();

        Assert.False(result);
    }

    [Fact]
    public void ShouldReturnFalseForAllButOneUnlocked()
    {
        Player.CheeseUnlocked = RecipeRepository.Recipes.Count - 2;

        var result = Player.HasUnlockedAllRecipes();

        Assert.False(result);
    }

    [Fact]
    public void ShouldReturnTrueForAllRecipeUnlocked()
    {
        Player.CheeseUnlocked = RecipeRepository.Recipes.Count - 1;

        var result = Player.HasUnlockedAllRecipes();

        Assert.True(result);
    }
}
