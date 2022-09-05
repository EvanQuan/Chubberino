﻿using Chubberino.Bots.Channel.Modules.CheeseGame.Hazards;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items;
using Chubberino.Database.Models;
using System;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Quests;

public static class PlayerQuestExtensions
{
    public static Double GetRareQuestChance(this Player player)
    {
        return player.NextQuestUpgradeUnlock.GetRareQuestChance();
    }

    public static Boolean HasQuestingUnlocked(this Player player)
    {
        return player.QuestsUnlockedCount > 0;
    }

    public static Double GetQuestSuccessChance(this Player player, Boolean includeInfestation = true)
    {
        Double baseSuccessChance = Quest.BaseSuccessChance;

        Double workerSuccessChance = includeInfestation && player.IsInfested() ? 0 : player.GearCount * Gear.QuestSuccessBonus;

        return baseSuccessChance + workerSuccessChance;
    }
}
