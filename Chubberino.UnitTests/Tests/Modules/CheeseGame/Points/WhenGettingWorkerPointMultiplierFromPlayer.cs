using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Rankings;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points
{
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
}
