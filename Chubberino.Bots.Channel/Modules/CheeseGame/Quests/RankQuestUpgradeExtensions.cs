using Chubberino.Database.Models;
using System;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Quests;

public static class RankQuestUpgradeExtensions
{
    /// <summary>
    /// Base chance for rare quests.
    /// </summary>
    public const Double BaseRareQuestChance = 0.01;

    /// <summary>
    /// Chance increase per rare quest reward.
    /// </summary>
    public const Double RareQuestUpgradePercent = 0.005;

    /// <summary>
    /// Gets the chance for a rare quest to be chosen.
    /// </summary>
    /// <param name="rank"></param>
    /// <returns></returns>
    public static Double GetRareQuestChance(this Rank rank)
    {
        return BaseRareQuestChance + (Int32)rank * RareQuestUpgradePercent;
    }
}
