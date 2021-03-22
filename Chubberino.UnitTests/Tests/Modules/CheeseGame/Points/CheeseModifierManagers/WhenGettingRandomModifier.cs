using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.UnitTests.Utility;
using Moq;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points.CheeseModifierManagers
{
    public sealed class WhenGettingRandomModifier : UsingCheeseModifierManager
    {
        [Theory]
        [InlineData((Rank)(Rank.Bronze - 1))]
        [InlineData(Rank.Bronze)]
        public void ShouldReturnNull(Rank rank)
        {
            MockedRandom.SetupReturnMaximum();

            var modifier = Sut.GetRandomModifier(rank);

            Assert.Null(modifier);
        }

        [Theory]
        [InlineData((Rank)(Rank.Bronze - 1))]
        [InlineData(Rank.Bronze)]
        [InlineData(Rank.Silver)]
        [InlineData(Rank.Gold)]
        [InlineData(Rank.Platinum)]
        [InlineData(Rank.Diamond)]
        [InlineData(Rank.Master)]
        [InlineData(Rank.Grandmaster)]
        [InlineData(Rank.Legend)]
        [InlineData(Rank.None)]
        [InlineData((Rank)(Rank.None + 1))]
        public void ShouldReturnMinimumForAllRanks(Rank rank)
        {
            MockedRandom.SetupReturnMinimum();

            var modifier = Sut.GetRandomModifier(rank);

            Assert.Null(modifier);
        }

        [Fact]
        public void ShouldReturnFirstModifier()
        {
            MockedRandom.SetupReturnMaximum();

            var modifier = Sut.GetRandomModifier(Rank.Silver);

            Assert.NotNull(modifier);
            Assert.Equal(CheeseModifierManager.Modifiers[1], modifier);
        }

        [Theory]
        [InlineData(Rank.None)]
        [InlineData((Rank)(Rank.None + 1))]
        public void ShouldReturnFirstLast(Rank rank)
        {
            MockedRandom.SetupReturnMaximum();

            var modifier = Sut.GetRandomModifier(rank);

            Assert.NotNull(modifier);
            Assert.Equal(CheeseModifierManager.Modifiers[CheeseModifierManager.Modifiers.Count - 1], modifier);
        }

    }
}
