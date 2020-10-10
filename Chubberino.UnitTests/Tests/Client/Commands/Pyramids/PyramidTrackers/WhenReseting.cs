using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids
{
    public sealed class WhenReseting : UsingPyramidTracker
    {
        public WhenReseting()
        {
            Sut.Start(ExpectedName1, ExpectedBlock1);
        }

        [Fact]
        public void ShouldResetValuesForSingleContributor()
        {
            Sut.Reset();

            Assert.Empty(Sut.ContributorDisplayNames);
            Assert.Equal(0, Sut.CurrentHeight);
            Assert.Equal(0, Sut.TallestHeight);
            Assert.Null(Sut.Block);
            Assert.False(Sut.HasStarted);
        }

        [Fact]
        public void ShouldResetValuesForMultipleContributors()
        {
            Sut.BuildUp(ExpectedName2);

            Sut.Reset();

            Assert.Empty(Sut.ContributorDisplayNames);
            Assert.Equal(0, Sut.CurrentHeight);
            Assert.Equal(0, Sut.TallestHeight);
            Assert.Null(Sut.Block);
            Assert.False(Sut.HasStarted);
        }
    }
}
