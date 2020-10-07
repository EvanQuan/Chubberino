using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids
{
    public sealed class WhenReseting : UsingPyramidTracker
    {
        [Fact]
        public void ShouldResetValuesForSingleContributor()
        {
            Sut.Start("name", "block");

            Sut.Reset();

            Assert.Empty(Sut.ContributorDisplayNames);
            Assert.Equal(0, Sut.CurrentHeight);
            Assert.Equal(0, Sut.TallestHeight);
            Assert.Null(Sut.Block);
        }

        [Fact]
        public void ShouldResetValuesForMultipleContributors()
        {
            Sut.Start("name", "block");

            Sut.BuildUp("name2");

            Sut.Reset();

            Assert.Empty(Sut.ContributorDisplayNames);
            Assert.Equal(0, Sut.CurrentHeight);
            Assert.Equal(0, Sut.TallestHeight);
            Assert.Null(Sut.Block);
        }
    }
}
