using Chubberino.Bots.Channel.Modules.CheeseGame.Hazards;
using Chubberino.Database.Models;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Hazards;

public sealed class WhenCheckingIfPlayerIsInfested
{
    private Player Player { get; }

    public WhenCheckingIfPlayerIsInfested()
    {
        Player = new();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(Int32.MinValue)]
    public void ShouldReturnFalse(Int32 ratCount)
    {
        Player.RatCount = ratCount;

        var result = Player.IsInfested();

        Assert.False(result);
    }

    [Theory]
    [InlineData(Int32.MaxValue)]
    [InlineData(1)]
    public void ShouldReturnTrue(Int32 ratCount)
    {
        Player.RatCount = ratCount;

        var result = Player.IsInfested();

        Assert.True(result);
    }
}
