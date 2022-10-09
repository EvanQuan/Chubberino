using Chubberino.Bots.Channel.Modules.CheeseGame.Heists;
using Chubberino.Database.Models;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Heists;

public sealed class WhenTryGettingWager
{
    [Theory]
    [InlineData("1", 1, 1)]
    [InlineData("1", 0, 1)]
    [InlineData("1k", 0, 1000)]
    [InlineData("1K", 0, 1000)]
    [InlineData("1.5k", 0, 1500)]
    [InlineData("1.5K", 0, 1500)]
    [InlineData("50%", 10, 5)]
    [InlineData("50%", 3, 2)]
    [InlineData("0.5%", 100, 1)]
    [InlineData("10.1%", 1000, 101)]
    [InlineData("all", 2, 2)]
    [InlineData("ALL", 2, 2)]
    [InlineData("aLl", 2, 2)]
    [InlineData("all", 0, 0)]
    [InlineData("ALL", 0, 0)]
    [InlineData("aLl", 0, 0)]
    [InlineData("a", 2, 2)]
    [InlineData("A", 2, 2)]
    [InlineData("a", 0, 0)]
    [InlineData("A", 0, 0)]
    [InlineData("none", 0, 0)]
    [InlineData("NONE", 0, 0)]
    [InlineData("nOnE", 0, 0)]
    [InlineData("none", 1, 0)]
    [InlineData("NONE", 1, 0)]
    [InlineData("nOnE", 1, 0)]
    [InlineData("n", 0, 0)]
    [InlineData("N", 0, 0)]
    [InlineData("n", 1, 0)]
    [InlineData("N", 1, 0)]
    [InlineData("leave", 0, 0)]
    [InlineData("LEAVE", 0, 0)]
    [InlineData("lEaVe", 0, 0)]
    [InlineData("leave", 1, 0)]
    [InlineData("LEAVE", 1, 0)]
    [InlineData("lEaVe", 1, 0)]
    [InlineData("l", 0, 0)]
    [InlineData("L", 0, 0)]
    [InlineData("l", 1, 0)]
    [InlineData("L", 1, 0)]
    public void ShouldGetWager(String proposedWager, Int32 playerPoints, Int32 expectedWager)
    {
        var player = new Player()
        {
            Points = playerPoints
        };

        Boolean result = proposedWager.TryGetWager(out var wager);

        Assert.True(result);
        Assert.Equal(expectedWager, wager(player));
    }

    [Theory]
    [InlineData("")]
    [InlineData("b")]
    [InlineData("10a")]
    [InlineData("1.1")]
    [InlineData("%")]
    [InlineData("a%")]
    [InlineData("k")]
    [InlineData("k1")]
    [InlineData("ak")]
    [InlineData("K")]
    public void ShouldFailToGetWager(String proposedWager)
    {
        var player = new Player();

        Boolean result = proposedWager.TryGetWager(out var wager);

        Assert.False(result);
        Assert.Null(wager);
    }
}
