using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Workers;
using Chubberino.Database.Models;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points;

public sealed class WhenGettingWorkerPointMultiplierFromPlayer
{
    private Player Player { get; }

    public WhenGettingWorkerPointMultiplierFromPlayer()
    {
        Player = new();
    }

    [Fact]
    public void ShouldReturnBaseMultiplier()
    {
        Double multiplier = Player.GetWorkerPointMultiplier();

        Assert.Equal(RankWorkerUpgradeExtensions.BaseWorkerPointPercent, multiplier);
    }

    [Fact]
    public void ShouldAddRankUpgradeMultiplier()
    {
        Player.NextWorkerProductionUpgradeUnlock = Rank.Silver;

        Double multiplier = Player.GetWorkerPointMultiplier();

        Double expectedMultiplier = RankWorkerUpgradeExtensions.BaseWorkerPointPercent + RankWorkerUpgradeExtensions.WorkerUpgradePercent;

        Assert.Equal(expectedMultiplier, multiplier);
    }
}
