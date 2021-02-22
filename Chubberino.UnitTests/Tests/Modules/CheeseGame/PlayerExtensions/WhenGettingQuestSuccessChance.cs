using Chubberino.Modules.CheeseGame;
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
            Double result = Player.GetQuestSuccessChance();

            Assert.Equal(Constants.QuestBaseSuccessChance, result);
        }

        [Theory]
        [InlineData(0, Rank.Bronze, 1, Rank.Bronze)]
        [InlineData(0, Rank.Bronze, 2, Rank.Silver)]
        [InlineData(1, Rank.Silver, 2, Rank.Silver)]
        [InlineData(1, Rank.Silver, 1, Rank.Gold)]
        public void ShouldAddWorkerBonus(Int32 lesserWorkerCount, Rank lesserWorkerUpgrade, Int32 greaterWorkerCount, Rank greaterWorkerUpgrade)
        {
            Player.WorkerCount = lesserWorkerCount;
            Player.NextQuestSuccessUpgradeUnlock = lesserWorkerUpgrade;

            Double lesserResult = Player.GetQuestSuccessChance();

            Player.WorkerCount = greaterWorkerCount;
            Player.NextQuestSuccessUpgradeUnlock = greaterWorkerUpgrade;

            Double greaterResult = Player.GetQuestSuccessChance();

            Assert.True(lesserResult < greaterResult);
        }
    }
}
