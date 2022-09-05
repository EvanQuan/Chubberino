using System;
using System.Linq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids.PyramidTrackers;

public sealed class WhenBuildingUp : UsingPyramidTracker
{
    public WhenBuildingUp()
    {
        Sut.Start(ExpectedName1, ExpectedBlock1);
    }

    [Fact]
    public void ShouldIncreaseHeight()
    {
        Sut.BuildUp(ExpectedName1);

        Assert.Single(Sut.ContributorDisplayNames);
        Assert.Equal(ExpectedName1, Sut.ContributorDisplayNames.Single());
        Assert.Equal(ExpectedBlock1, Sut.Block);
        Assert.Equal(2, Sut.TallestHeight);
        Assert.Equal(2, Sut.CurrentHeight);
        Assert.True(Sut.HasStarted);
    }

    [Fact]
    public void ShouldAddContributor()
    {
        Sut.BuildUp(ExpectedName2);

        Assert.Equal(2, Sut.ContributorDisplayNames.Count);
        Assert.Equal(new String[] { ExpectedName1, ExpectedName2 }, Sut.ContributorDisplayNames);
        Assert.Equal(ExpectedBlock1, Sut.Block);
        Assert.Equal(2, Sut.TallestHeight);
        Assert.Equal(2, Sut.CurrentHeight);
        Assert.True(Sut.HasStarted);
    }
}
