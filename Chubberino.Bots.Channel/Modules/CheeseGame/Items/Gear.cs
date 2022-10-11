using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items;

public sealed class Gear : Item
{
    /// <summary>
    /// The additive quest success percent bonus provided by each gear.
    /// </summary>
    public const Double QuestSuccessBonus = 0.005;

    public const Int32 MaximumCount = 150;

    public override IEnumerable<String> Names { get; } = new String[]
    {
        "Gear",
        "g",
        "gears"
    };

    public override Either<Int32, String> GetPrice(Player player)
        => player.GetGearPrice();

    public override String GetSpecificNameForNotEnoughToBuy(Player player) => "some gear";

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


    public override Option<String> IsForSale(Player player)
    {
        if (player.GearCount < 150)
        {
            return Option<String>.None;
        }

        return "You cannot buy any more gear.";
    }

    public override Option<String> GetShopPrompt(Player player)
        => IsForSale(player)
            .Some(_ => Option<String>.None)
            .None(() =>
            {
                var questSuccessChance = player.GetQuestSuccessChance(includeInfestation: false);
                return $"{base.GetShopPrompt(player)} [+{String.Format("{0:0.0}", questSuccessChance * 100)}% -> +{String.Format("{0:0.0}", (questSuccessChance + QuestSuccessBonus) * 100)}% quest success] for {GetPrice(player)} cheese";
            });
}
