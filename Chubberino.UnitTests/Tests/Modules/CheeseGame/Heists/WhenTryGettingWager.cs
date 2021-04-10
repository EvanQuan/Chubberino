using Chubberino.Modules.CheeseGame.Heists;
using Chubberino.Modules.CheeseGame.Models;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Heists
{
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
        public void ShouldGetWager(String proposedWager, Int32 playerPoints, Int32 expectedWager)
        {
            var player = new Player()
            {
                Points = playerPoints
            };

            Boolean result = player.TryGetWager(proposedWager, out Int32 wager);

            Assert.True(result);
            Assert.Equal(expectedWager, wager);
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

            Boolean result = player.TryGetWager(proposedWager, out Int32 wager);

            Assert.False(result);
            Assert.Equal(0, wager);
        }
    }
}
