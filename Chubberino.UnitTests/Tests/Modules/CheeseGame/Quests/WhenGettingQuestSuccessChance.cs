﻿using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Database.Models;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Quests;

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

        Assert.Equal(Quest.BaseSuccessChance, result);
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
