using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids
{
    public sealed class WhenStarting : UsingPyramidTracker
    {
        [Fact]
        public void ShouldStartNewPyramid()
        {
            const String expectedName = "name1";
            const String expectedBlock = "block";

            Sut.Start(expectedName, expectedBlock);

            Assert.Single(Sut.ContributorDisplayNames);
            Assert.Equal(expectedName, Sut.ContributorDisplayNames.Single());
            Assert.Equal(expectedBlock, Sut.Block);
            Assert.Equal(1, Sut.TallestHeight);
            Assert.Equal(1, Sut.CurrentHeight);
        }

        [Fact]
        public void ShouldOverrideOldPyramid()
        {
            const String expectedName = "name1";
            const String expectedBlock = "block";

            Sut.Start("nameOld", "blockOld");

            Sut.Start(expectedName, expectedBlock);

            Assert.Single(Sut.ContributorDisplayNames);
            Assert.Equal(expectedName, Sut.ContributorDisplayNames.Single());
            Assert.Equal(expectedBlock, Sut.Block);
            Assert.Equal(1, Sut.TallestHeight);
            Assert.Equal(1, Sut.CurrentHeight);
        }
    }
}
