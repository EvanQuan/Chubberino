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
        [InlineData(0, Rank.Silver, 2, Rank.Bronze)]
        [InlineData(1, Rank.Silver, 2, Rank.Silver)]
        [InlineData(1, Rank.Silver, 2, Rank.Gold)]
        [InlineData(1, Rank.Gold, 2, Rank.Silver)]
        public void ShouldAddWorkerBonus(Int32 lesserWorkerCount, Rank lesserWorkerUpgrade, Int32 greaterWorkerCount, Rank greaterWorkerUpgrade)
        {
            Player.WorkerCount = lesserWorkerCount;
            Player.NextQuestRewardUpgradeUnlock = lesserWorkerUpgrade;

            Double lesserResult = Player.GetQuestSuccessChance();

            Player.WorkerCount = greaterWorkerCount;
            Player.NextQuestRewardUpgradeUnlock = greaterWorkerUpgrade;

            Double greaterResult = Player.GetQuestSuccessChance();

            Assert.True(lesserResult < greaterResult);
        }

        [Theory]
        [InlineData(0, Rank.Bronze, Rank.Bronze)]
        [InlineData(0, Rank.Bronze, Rank.Silver)]
        [InlineData(1, Rank.Silver, Rank.Silver)]
        [InlineData(1, Rank.Silver, Rank.Gold)]
        public void ShouldNotBeImpactedByRank(Int32 workerCount, Rank lessRanker, Rank greaterRank)
        {
            Player.WorkerCount = workerCount;
            Player.NextQuestRewardUpgradeUnlock = lessRanker;

            Double lesserResult = Player.GetQuestSuccessChance();

            Player.NextQuestRewardUpgradeUnlock = greaterRank;

            Double greaterResult = Player.GetQuestSuccessChance();

            Assert.Equal(lesserResult,  greaterResult);
        }
    }
}
