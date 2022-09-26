using System;
using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Hazards;
using Chubberino.Database.Models;
using LanguageExt;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items;

public sealed class Mousetrap : Item
{
    public override IEnumerable<String> Names { get; } = new String[] { "Mousetrap", "m", "mouse", "mousetraps" };

    public override Int32 GetPrice(Player player)
    {
        return player.GetMousetrapPrice();
    }

    public override String GetSpecificNameForNotEnoughToBuy(Player player)
    {
        return "a mousetrap";
    }

    public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
    {
        return $"{quantity} mousetrap{(quantity == 1 ? String.Empty : "s")}";
    }

    public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
    {
        player.MouseTrapCount++;
        player.Points -= price;

        return 1;
    }

    public override String GetShopPrompt(Player player)
    {
        return $"{base.GetShopPrompt(player)} [+1] for {GetPrice(player)} cheese";
    }

    public override String OnAfterBuy(Player player, Int32 quantityPurchased, Int32 pointsSpent)
    {
        if (!player.IsInfested()) { return String.Empty; }

        Boolean isSingleMousetrapUsed = player.RatCount == 1;

        if (quantityPurchased >= player.RatCount)
        {
            // Eliminated all the rats.
            var outputMessage = $"You set up {(isSingleMousetrapUsed ? "a mousetrap" : $"{player.RatCount} mousetraps")}, killing {(isSingleMousetrapUsed ? "the giant rat" : $"all the giant rats")} infesting your cheese factory. " +
                $"Your workers go back to work. ";

            player.MouseTrapCount -= player.RatCount;
            player.RatCount = 0;

            return outputMessage;
        }
        else if (quantityPurchased > 0)
        {

            Int32 newRatCount = player.RatCount - player.MouseTrapCount;

            var outputMessage = $"You set up {(isSingleMousetrapUsed ? "a mousetrap" : $"{player.RatCount} mousetraps")}, killing {(isSingleMousetrapUsed ? "a giant rat" : "some of the giant rats")} infesting your cheese factory. {newRatCount} {(newRatCount == 1 ? "remains" : "remain")}, scaring away your workers. ";

            // Eliminated some of the the rats.
            player.MouseTrapCount = 0;
            player.RatCount = newRatCount;

            return outputMessage;
        }
        else
        {
            String rat = isSingleMousetrapUsed ? "rat" : "rats";

            // Eliminated no rats.
            return $"{player.RatCount} giant {rat} {(isSingleMousetrapUsed ? "is" : "are")} still infesting your cheese factory, scaring away your workers. ";
        }
    }
}
