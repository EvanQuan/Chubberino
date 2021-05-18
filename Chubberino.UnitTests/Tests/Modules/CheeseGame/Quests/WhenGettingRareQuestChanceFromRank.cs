using Chubberino.Modules.CheeseGame.Quests;
using Chubberino.Modules.CheeseGame.Ranks;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Quests
{
    public sealed class WhenGettingRareQuestChanceFromRank
    {
        [Fact]
        public void ShouldReturnBaseChance()
        {
            Double chance = Rank.Bronze.GetRareQuestChance();

            Assert.Equal(RankQuestUpgradeExtensions.BaseRareQuestChance, chance);
        }

        [Fact]
        public void ShouldAddRankUpgradeChance()
        {
            Double chance = Rank.Silver.GetRareQuestChance();

            Double expectedChance = RankQuestUpgradeExtensions.BaseRareQuestChance + RankQuestUpgradeExtensions.RareQuestUpgradePercent;

            Assert.Equal(expectedChance, chance);
        }
    }
}
