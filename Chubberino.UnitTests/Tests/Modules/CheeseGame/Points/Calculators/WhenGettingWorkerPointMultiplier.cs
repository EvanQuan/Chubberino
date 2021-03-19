using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Rankings;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points.Calculators
{
    public sealed class WhenGettingWorkerPointMultiplier
    {
        private ICalculator Sut { get; }

        public WhenGettingWorkerPointMultiplier()
        {
            Sut = new Calculator();
        }

        [Fact]
        public void ShouldReturnBaseMultiplier()
        {
            Double multiplier = Sut.GetWorkerPointMultiplier(Rank.Bronze);

            Assert.Equal(Calculator.BaseWorkerPointPercent, multiplier);
        }

        [Fact]
        public void ShouldAddRankUpgradeMultiplier()
        {
            Double multiplier = Sut.GetWorkerPointMultiplier(Rank.Silver);

            Double expectedMultiplier = Calculator.BaseWorkerPointPercent + Calculator.WorkerUpgradePercent;

            Assert.Equal(expectedMultiplier, multiplier);
        }
    }
}
