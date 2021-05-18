using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Rankings;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points
{
    public sealed class WhenGettingWorkerPointMultiplier
    {
        [Fact]
        public void ShouldReturnBaseMultiplier()
        {
            Double multiplier = Rank.Bronze.GetWorkerPointMultiplier();

            Assert.Equal(RankWorkerUpgradeExtensions.BaseWorkerPointPercent, multiplier);
        }

        [Fact]
        public void ShouldAddRankUpgradeMultiplier()
        {
            Double multiplier = Rank.Silver.GetWorkerPointMultiplier();

            Double expectedMultiplier = RankWorkerUpgradeExtensions.BaseWorkerPointPercent + RankWorkerUpgradeExtensions.WorkerUpgradePercent;

            Assert.Equal(expectedMultiplier, multiplier);
        }
    }
}
