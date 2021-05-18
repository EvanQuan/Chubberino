using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Quests;
using Chubberino.Modules.CheeseGame.Rankings;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Quests
{
    public sealed class WhenGettingRareQuestChanceFromPlayer
    {
        private Player Player { get; }

        public WhenGettingRareQuestChanceFromPlayer()
        {
            Player = new();
        }

        [Fact]
        public void ShouldReturnBaseChance()
        {
            Double chance = Player.GetRareQuestChance();

            Assert.Equal(RankQuestUpgradeExtensions.BaseRareQuestChance, chance);
        }

        [Fact]
        public void ShouldAddRankUpgradeChance()
        {
            Player.NextQuestUpgradeUnlock = Rank.Silver;

            Double chance = Player.GetRareQuestChance();

            Double expectedChance = RankQuestUpgradeExtensions.BaseRareQuestChance + RankQuestUpgradeExtensions.RareQuestUpgradePercent;

            Assert.Equal(expectedChance, chance);
        }
    }
}
