using System;
using System.Linq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids
{
    public sealed class WhenStarting : UsingPyramidTracker
    {
        [Fact]
        public void ShouldStartNewPyramid()
        {
            Sut.Start(ExpectedName1, ExpectedBlock1);

            Assert.Single(Sut.ContributorDisplayNames);
            Assert.Equal(ExpectedName1, Sut.ContributorDisplayNames.Single());
            Assert.Equal(ExpectedBlock1, Sut.Block);
            Assert.Equal(1, Sut.TallestHeight);
            Assert.Equal(1, Sut.CurrentHeight);
        }

        [Fact]
        public void ShouldOverrideOldPyramid()
        {
            Sut.Start(ExpectedName1, ExpectedBlock1);

            Sut.Start(ExpectedName2, ExpectedBlock2);

            Assert.Single(Sut.ContributorDisplayNames);
            Assert.Equal(ExpectedName2, Sut.ContributorDisplayNames.Single());
            Assert.Equal(ExpectedBlock2, Sut.Block);
            Assert.Equal(1, Sut.TallestHeight);
            Assert.Equal(1, Sut.CurrentHeight);
        }
    }
}
