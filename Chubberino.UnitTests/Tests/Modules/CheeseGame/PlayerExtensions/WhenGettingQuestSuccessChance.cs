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
        [InlineData(0, 1)]
        [InlineData(0, 2)]
        [InlineData(1, 2)]
        public void ShouldAddGearBonus(Int32 lesserGearCount, Int32 greaterGearCount)
        {
            Player.GearCount = lesserGearCount;

            Double lesserResult = Player.GetQuestSuccessChance();

            Player.GearCount = greaterGearCount;

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
            Player.NextQuestUpgradeUnlock = lessRanker;

            Double lesserResult = Player.GetQuestSuccessChance();

            Player.NextQuestUpgradeUnlock = greaterRank;

            Double greaterResult = Player.GetQuestSuccessChance();

            Assert.Equal(lesserResult,  greaterResult);
        }
    }
}
