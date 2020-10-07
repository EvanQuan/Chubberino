using System;
using System.Linq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids
{
    public sealed class WhenBuildingUp : UsingPyramidTracker
    {
        public const String ExpectedName = "name";
        public const String ExpectedBlock = "block";

        public WhenBuildingUp()
        {
            Sut.Start(ExpectedName, ExpectedBlock);
        }

        [Fact]
        public void ShouldIncreaseHeight()
        {
            Sut.BuildUp(ExpectedName);

            Assert.Single(Sut.ContributorDisplayNames);
            Assert.Equal(ExpectedName, Sut.ContributorDisplayNames.Single());
            Assert.Equal(ExpectedBlock, Sut.Block);
            Assert.Equal(2, Sut.TallestHeight);
            Assert.Equal(2, Sut.CurrentHeight);
        }

        [Fact]
        public void ShouldAddContributor()
        {
            Sut.BuildUp("name2");

            Assert.Equal(2, Sut.ContributorDisplayNames.Count);
            Assert.Equal(new String[] { ExpectedName, "name2" }, Sut.ContributorDisplayNames);
            Assert.Equal(ExpectedBlock, Sut.Block);
            Assert.Equal(2, Sut.TallestHeight);
            Assert.Equal(2, Sut.CurrentHeight);
        }
    }
}
