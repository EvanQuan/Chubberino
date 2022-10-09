using Chubberino.Bots.Channel.Modules.CheeseGame.Points;
using Chubberino.Database.Models;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points;

public sealed class WhenAddingPoints
{
    private Player Player { get; }

    public WhenAddingPoints()
    {
        Player = new Player();
    }

    [Theory]
    [InlineData(1, 0, 0, 0, 0)]
    [InlineData(1, 0, 0, 1, 0)]
    [InlineData(1, 1, 1, 0, 1)]
    [InlineData(1, 1, 1, 1, 1)]
    public void ShouldCapAtMaximumStorage(Int32 pointGain, Int32 storage, Int32 initialPoints, Int32 workers, Int32 expectedPoints)
    {
        Player.MaximumPointStorage = storage;

        Player.WorkerCount = workers;
        Player.Points = initialPoints;

        Player.AddPoints(pointGain);

        Assert.Equal(expectedPoints, Player.Points);
    }


    [Theory]
    [InlineData(-1, 1, 0, 0)]
    [InlineData(-1, 1, 0, 1)]
    [InlineData(-2, 1, 1, 0)]
    [InlineData(-2, 1, 1, 1)]
    public void ShouldMinimizeToZero(Int32 pointGain, Int32 storage, Int32 initialPoints, Int32 workers)
    {
        Player.MaximumPointStorage = storage;

        Player.WorkerCount = workers;
        Player.Points = initialPoints;

        Player.AddPoints(pointGain);

        Assert.Equal(0, Player.Points);
    }


    [Theory]
    [InlineData(2, 2, -1, 1)]
    [InlineData(2, 0, 1, 1)]
    [InlineData(2, 1, 0, 1)]
    [InlineData(2, 1, -1, 0)]
    [InlineData(2, 0, -1, 0)]
    [InlineData(0, 0, 1, 0)]
    [InlineData(1, 0, 1, 1)]
    [InlineData(1, 0, 2, 1)]
    public void ShouldAddDirectPoints(Int32 storage, Int32 initialPoints, Int32 pointsToAdd, Int32 expectedFinalPoints)
    {
        Player.MaximumPointStorage = storage;
        Player.Points = initialPoints;
        Player.AddPoints(pointsToAdd);

        Int32 actualFinalPoints = Player.Points;

        Assert.Equal(expectedFinalPoints, actualFinalPoints);
    }
}
