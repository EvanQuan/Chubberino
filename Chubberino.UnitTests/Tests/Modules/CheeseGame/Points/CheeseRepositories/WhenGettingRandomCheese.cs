using Chubberino.Modules.CheeseGame.Points;
using Moq;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points.CheeseRepositories
{
    public sealed class WhenGettingRandomCheese : UsingCheeseRepository
    {
        public WhenGettingRandomCheese()
        {
            MockedRandom.Setup(x => x.Next(It.Is<Int32>(x => x < 0))).Throws<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ShouldReturnLowestCheese(Int32 cheeseUnlocked)
        {
            MockedRandom.Setup(x => x.Next(It.Is<Int32>(x => x >= 0))).Returns(0);

            var result = Sut.GetRandomType(cheeseUnlocked);

            var expectedCheese = CheeseRepository.Cheeses[0];

            Assert.Equal(expectedCheese, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ShouldReturnHighestUnlockedCheese(Int32 cheeseUnlocked)
        {
            MockedRandom.Setup(x => x.Next(It.IsAny<Int32>())).Returns<Int32>(x => x);

            var result = Sut.GetRandomType(cheeseUnlocked);

            var expectedCheese = CheeseRepository.Cheeses[cheeseUnlocked];

            Assert.Equal(expectedCheese, result);
        }

        [Fact]
        public void ShouldReturnHighestPossibleCheese()
        {
            Int32 maxCheeseUnlocked = CheeseRepository.Cheeses.Count - 1;

            MockedRandom.Setup(x => x.Next(It.IsAny<Int32>())).Returns<Int32>(x => x);

            var result = Sut.GetRandomType(maxCheeseUnlocked);

            var lastCheese = CheeseRepository.Cheeses[CheeseRepository.Cheeses.Count - 1];

            Assert.Equal(lastCheese, result);
        }

        [Fact]
        public void ShouldReturnHighestPossibleCheeseForInvalidHighCheeseUnlocked()
        {
            Int32 maxCheeseUnlocked = CheeseRepository.Cheeses.Count;

            MockedRandom.Setup(x => x.Next(It.IsAny<Int32>())).Returns<Int32>(x => x);

            var result = Sut.GetRandomType(maxCheeseUnlocked);

            var lastCheese = CheeseRepository.Cheeses[CheeseRepository.Cheeses.Count - 1];

            Assert.Equal(lastCheese, result);
        }

        [Fact]
        public void ShouldReturnLowestPossibleCheeseForInvalidLowCheeseUnlocked()
        {
            Int32 invalidCheeseUnlocked = -1;

            MockedRandom.Setup(x => x.Next(It.Is<Int32>(x => x >= 0))).Returns(0);

            var result = Sut.GetRandomType(invalidCheeseUnlocked);

            var firstCheese = CheeseRepository.Cheeses[0];

            Assert.Equal(firstCheese, result);
        }

    }
}
