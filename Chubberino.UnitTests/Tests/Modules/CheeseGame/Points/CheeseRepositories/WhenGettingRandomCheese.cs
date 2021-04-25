using Chubberino.Modules.CheeseGame.Points;
using Chubberino.UnitTests.Utility;
using Moq;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points.CheeseRepositories
{
    public sealed class WhenGettingRandomCheese : UsingCheeseRepository
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ShouldReturnLowestCheese(Int32 cheeseUnlocked)
        {
            MockedRandom.SetupReturnMinimum();

            var result = Sut.GetRandom(cheeseUnlocked);

            var expectedCheese = CheeseRepository.Cheeses[0];

            Assert.Equal(expectedCheese, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(26)]
        public void ShouldReturnHighestUnlockedCheese(Int32 cheeseUnlocked)
        {
            MockedRandom.SetupReturnMaximum();

            var result = Sut.GetRandom(cheeseUnlocked);

            var expectedCheese = CheeseRepository.Cheeses[cheeseUnlocked];

            Assert.Equal(expectedCheese, result);
        }

        [Fact]
        public void ShouldReturnHighestPossibleCheese()
        {
            Int32 maxCheeseUnlocked = CheeseRepository.Cheeses.Count - 1;

            MockedRandom.SetupReturnMaximum();

            var result = Sut.GetRandom(maxCheeseUnlocked);

            var lastCheese = CheeseRepository.Cheeses[CheeseRepository.Cheeses.Count - 1];

            Assert.Equal(lastCheese, result);
        }

        [Fact]
        public void ShouldReturnHighestPossibleCheeseForInvalidHighCheeseUnlocked()
        {
            Int32 maxCheeseUnlocked = CheeseRepository.Cheeses.Count;

            MockedRandom.SetupReturnMaximum();

            var result = Sut.GetRandom(maxCheeseUnlocked);

            var lastCheese = CheeseRepository.Cheeses[CheeseRepository.Cheeses.Count - 1];

            Assert.Equal(lastCheese, result);
        }

        [Fact]
        public void ShouldReturnLowestPossibleCheeseForInvalidLowCheeseUnlocked()
        {
            Int32 invalidCheeseUnlocked = -1;

            MockedRandom.SetupReturnMinimum();

            var result = Sut.GetRandom(invalidCheeseUnlocked);

            var firstCheese = CheeseRepository.Cheeses[0];

            Assert.Equal(firstCheese, result);
        }

    }
}
