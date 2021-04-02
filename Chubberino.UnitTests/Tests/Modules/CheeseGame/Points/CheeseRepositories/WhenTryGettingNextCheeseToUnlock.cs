using Chubberino.Modules.CheeseGame.Points;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points.CheeseRepositories
{
    public sealed class WhenTryGettingNextCheeseToUnlock : UsingCheeseRepository
    {
        [Fact]
        public void ShouldReturnFirstUnlockableCheese()
        {
            var result = Sut.TryGetNextToUnlock(Player, out CheeseType cheeseType);

            var firstCheeseToUnlock = CheeseRepository.Cheeses[1];
            Assert.True(result);
            Assert.NotNull(cheeseType);
            Assert.Equal(firstCheeseToUnlock, cheeseType);
        }

        [Fact]
        public void ShouldReturnLastUnlockableCheese()
        {
            Player.CheeseUnlocked = CheeseRepository.Cheeses.Count - 2;
            var result = Sut.TryGetNextToUnlock(Player, out CheeseType cheeseType);

            var lastCheese = CheeseRepository.Cheeses[CheeseRepository.Cheeses.Count - 1];
    
            Assert.True(result);
            Assert.NotNull(cheeseType);
            Assert.Equal(lastCheese, cheeseType);
        }

        [Fact]
        public void ShouldReturnFalse()
        {
            Player.CheeseUnlocked = CheeseRepository.Cheeses.Count - 1;
            var result = Sut.TryGetNextToUnlock(Player, out CheeseType cheeseType);

            Assert.False(result);
            Assert.Null(cheeseType);
        }
    }
}
