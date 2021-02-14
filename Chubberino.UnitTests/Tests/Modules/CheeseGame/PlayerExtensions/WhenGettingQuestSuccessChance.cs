using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Rankings;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.PlayerExtensions
{
    public sealed class WhenGettingQuestSuccessChance
    {
        private Player Player { get; set; }
        
        public WhenGettingQuestSuccessChance()
        {
            Player = new Player();
        }

        [Fact]
        public void ShouldReturnBaseChance()
        {
            var result = Player.GetQuestSuccessChance();

            Assert.Equal(0.25, result);
        }

        [Theory]
        [InlineData(1, Rank.Bronze, 0.2525)]
        [InlineData(2, Rank.Bronze, 0.2550)]
        [InlineData(1, Rank.Silver, 0.2550)]
        [InlineData(2, Rank.Silver, 0.26)]
        public void ShouldAddWorkerBonus(Int32 workerCount, Rank workerUpgrade, Double expectedChance)
        {
            Player.WorkerCount = workerCount;
            Player.LastWorkerQuestHelpUnlocked = workerUpgrade;

            var result = Player.GetQuestSuccessChance();

            Assert.Equal(expectedChance, result);
        }
    }
}
