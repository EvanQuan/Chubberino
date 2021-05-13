using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using System.Collections.Generic;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points.CheeseRepositories
{
    public sealed class WhenTryGettingNextCheeseTypeToUnlock
    {
        public IReadOnlyList<CheeseType> Sut = CheeseRepository.Cheeses;

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
}
