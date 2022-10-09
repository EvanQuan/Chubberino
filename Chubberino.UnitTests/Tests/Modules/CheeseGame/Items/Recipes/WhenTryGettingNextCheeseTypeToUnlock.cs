﻿using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Recipes;
using Chubberino.Database.Models;
using System.Collections.Generic;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items.Recipes;

public sealed class WhenTryGettingNextCheeseTypeToUnlock
{
    public IReadOnlyList<RecipeInfo> Sut = RecipeRepository.Recipes;

    private Player Player { get; }

    public WhenTryGettingNextCheeseTypeToUnlock()
    {
        Player = new Player();
    }

    [Fact]
    public void ShouldReturnSecondCheese()
    {
        var result = Sut.TryGetNextToUnlock(Player, out var cheeseType);

        Assert.True(result);
        Assert.NotNull(cheeseType);
        Assert.Equal(Sut[1], cheeseType);
    }

    [Fact]
    public void ShouldReturnLastCheese()
    {
        Player.CheeseUnlocked = Sut.Count - 2;
        var result = Sut.TryGetNextToUnlock(Player, out var cheeseType);

        Assert.True(result);
        Assert.NotNull(cheeseType);
        Assert.Equal(Sut[Sut.Count - 1], cheeseType);
    }

    [Fact]
    public void ShouldReturnNoCheese()
    {
        Player.CheeseUnlocked = Sut.Count;
        var result = Sut.TryGetNextToUnlock(Player, out var cheeseType);

        Assert.False(result);
        Assert.Null(cheeseType);
    }
}
