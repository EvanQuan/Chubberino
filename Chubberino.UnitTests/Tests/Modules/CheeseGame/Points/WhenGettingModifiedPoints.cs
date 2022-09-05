using System;
using Chubberino.Bots.Channel.Modules.CheeseGame.Points;
using Chubberino.Database.Models;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points;

public sealed class WhenGettingModifiedPoints
{
    private Player Player { get; }

    public WhenGettingModifiedPoints()
    {
        Player = new Player();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(-1)]
    public void ShouldAddPointsWithoutWorkers(Int32 pointGain)
    {
        var modifiedPoints = Player.GetModifiedPoints(pointGain);

        Assert.Equal(pointGain, modifiedPoints);
    }

    /// <summary>
    /// If the worker point gain is collectively less than 1, will give 1 at minimum.
    /// </summary>
    [Theory]
    [InlineData(1, 2)]
    [InlineData(-1, -2)]
    public void ShouldAddMinimumWorkerPoints(Int32 pointGain, Int32 expectedPoints)
    {
        Player.WorkerCount = 1;

        var modifiedPoints = Player.GetModifiedPoints(pointGain);

        Assert.Equal(expectedPoints, modifiedPoints);
    }

    [Theory]
    [InlineData(100, 1, 110)]
    [InlineData(100, 2, 120)]
    [InlineData(-100, 1, -110)]
    [InlineData(-100, 2, -120)]
    public void ShouldAddWorkerPoints(Int32 pointGain, Int32 workers, Int32 expectedPoints)
    {
        Player.WorkerCount = workers;

        var modifiedPoints = Player.GetModifiedPoints(pointGain);

        Assert.Equal(expectedPoints, modifiedPoints);
    }
}
