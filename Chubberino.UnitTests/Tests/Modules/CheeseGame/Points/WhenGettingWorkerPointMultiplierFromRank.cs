using Chubberino.Modules.CheeseGame.Items.Workers;
using Chubberino.Modules.CheeseGame.Rankings;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points
{
    public sealed class WhenGettingWorkerPointMultiplierFromRank
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
