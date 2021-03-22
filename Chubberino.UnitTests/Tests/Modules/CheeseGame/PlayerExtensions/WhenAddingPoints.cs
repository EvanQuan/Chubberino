using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Points;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.PlayerExtensions
{
    public sealed class WhenAddingPoints
    {
        private Player Player { get; }
        private ICalculator Calculator { get; }
        // private CheeseType Cheese { get; }

        public WhenAddingPoints()
        {
            Player = new Player();
            Calculator = new Calculator();
        }

        [Theory]
        [InlineData(0, 1, 0, 0)]
        [InlineData(1, 1, 0, 1)]
        [InlineData(1, 2, 0, 1)]
        [InlineData(2, 3, 0, 2)]
        [InlineData(-1, 2, 1, 0)]
        [InlineData(-1, 2, 2, 1)]
        public void ShouldAddPointsWithoutWorkers(Int32 pointGain, Int32 storage, Int32 initialPoints, Int32 expectedPoints)
        {
            var cheese = new CheeseType(null, pointGain);

            Player.MaximumPointStorage = storage;

            Player.Points = initialPoints;

            Player.AddPoints(cheese, Calculator);

            Assert.Equal(expectedPoints, Player.Points);
        }

        /// <summary>
        /// If the worker point gain is collectively less than 1, will give 1 at minimum.
        /// </summary>
        [Theory]
        [InlineData(1, 2, 0, 2)]
        [InlineData(-1, 2, 2, 0)]
        public void ShouldAddMinimumWorkerPoints(Int32 pointGain, Int32 storage, Int32 initialPoints, Int32 expectedPoints)
        {
            var cheese = new CheeseType(null, pointGain);

            Player.MaximumPointStorage = storage;

            Player.WorkerCount = 1;
            Player.Points = initialPoints;

            Player.AddPoints(cheese, Calculator);

            Assert.Equal(expectedPoints, Player.Points);
        }

        [Theory]
        [InlineData(1, 0, 0, 0, 0)]
        [InlineData(1, 0, 0, 1, 0)]
        [InlineData(1, 1, 1, 0, 1)]
        [InlineData(1, 1, 1, 1, 1)]
        public void ShouldCapAtMaximumStorage(Int32 pointGain, Int32 storage, Int32 initialPoints, Int32 workers, Int32 expectedPoints)
        {
            var cheese = new CheeseType(null, pointGain);

            Player.MaximumPointStorage = storage;

            Player.WorkerCount = workers;
            Player.Points = initialPoints;

            Player.AddPoints(cheese, Calculator);

            Assert.Equal(expectedPoints, Player.Points);
        }

        [Theory]
        [InlineData(-1, 1, 0, 0)]
        [InlineData(-1, 1, 0, 1)]
        [InlineData(-2, 1, 1, 0)]
        [InlineData(-2, 1, 1, 1)]
        public void ShouldMinimizeToZero(Int32 pointGain, Int32 storage, Int32 initialPoints, Int32 workers)
        {
            var cheese = new CheeseType(null, pointGain);

            Player.MaximumPointStorage = storage;

            Player.WorkerCount = workers;
            Player.Points = initialPoints;

            Player.AddPoints(cheese, Calculator);

            Assert.Equal(0, Player.Points);
        }

        [Theory]
        [InlineData(100, 200, 0, 1, 110)]
        [InlineData(100, 200, 0, 2, 120)]
        [InlineData(-100, 200, 200, 1, 90)]
        [InlineData(-100, 200, 200, 2, 80)]
        public void ShouldAddWorkerPoints(Int32 pointGain, Int32 storage, Int32 initialPoints, Int32 workers, Int32 expectedPoints)
        {
            
            var cheese = new CheeseType(null, pointGain);

            Player.MaximumPointStorage = storage;

            Player.WorkerCount = workers;
            Player.Points = initialPoints;

            Player.AddPoints(cheese, Calculator);

            Assert.Equal(expectedPoints, Player.Points);
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
}
