using System;
using System.Linq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids
{
    public sealed class WhenBuildingDown : UsingPyramidTracker
    {
        public WhenBuildingDown()
        {
            Sut.Start(ExpectedName1, ExpectedBlock1);
            Sut.BuildUp(ExpectedName1);
        }

        [Fact]
        public void ShouldDecreaseCurrentHeight()
        {
            Sut.BuildDown(ExpectedName1);

            Assert.Single(Sut.ContributorDisplayNames);
            Assert.Equal(ExpectedName1, Sut.ContributorDisplayNames.Single());
            Assert.Equal(ExpectedBlock1, Sut.Block);
            Assert.Equal(2, Sut.TallestHeight);
            Assert.Equal(1, Sut.CurrentHeight);
            Assert.True(Sut.HasStarted);
        }

        [Fact]
        public void ShouldAddContributor()
        {
            Sut.BuildDown(ExpectedName2);

            Assert.Equal(2, Sut.ContributorDisplayNames.Count);
            Assert.Equal(new String[] { ExpectedName1, ExpectedName2 }, Sut.ContributorDisplayNames);
            Assert.Equal(ExpectedBlock1, Sut.Block);
            Assert.Equal(2, Sut.TallestHeight);
            Assert.Equal(1, Sut.CurrentHeight);
            Assert.True(Sut.HasStarted);
        }
    }
}
