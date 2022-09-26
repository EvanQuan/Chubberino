using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Database.Models;
using LanguageExt;
using System;
using System.Collections.Generic;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items;

public sealed class Gear : Item
{
    /// <summary>
    /// The additive quest success percent bonus provided by each gear.
    /// </summary>
    public const Double QuestSuccessBonus = 0.005;

    public const Int32 MaximumCount = 150;

    public override IEnumerable<String> Names { get; } = new String[] { "Gear", "g", "gears" };

    public override Int32 GetPrice(Player player)
    {
        return player.GetGearPrice();
    }

    public override String GetSpecificNameForNotEnoughToBuy(Player player)
    {
        return "some gear";
    }

    public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
    {
        var newQuestSuccessChance = player.GetQuestSuccessChance(includeInfestation: false);
        var oldQuestSuccessChance = newQuestSuccessChance - quantity * QuestSuccessBonus;
        return $"{quantity} gear [{String.Format("{0:0.0}", oldQuestSuccessChance * 100)}% -> {String.Format("{0:0.0}", newQuestSuccessChance * 100)}% quest success]";
    }

    public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
    {
        player.GearCount++;
        player.Points -= price;

        return 1;
    }


    public override Boolean IsForSale(Player player, out String reason)
    {
        if (player.GearCount < 150)
        {
            reason = default;
            return true;
        }

        reason = "You cannot buy any more gear.";
        return false;
    }

    public override String GetShopPrompt(Player player)
    {
        if (IsForSale(player, out _))
        {
            var questSuccessChance = player.GetQuestSuccessChance(includeInfestation: false);
            return $"{base.GetShopPrompt(player)} [+{String.Format("{0:0.0}", questSuccessChance * 100)}% -> +{String.Format("{0:0.0}", (questSuccessChance + QuestSuccessBonus) * 100)}% quest success] for {GetPrice(player)} cheese";
        }

        return null;
    }
}
