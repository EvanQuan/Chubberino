using System;
using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items;
using Chubberino.Database.Models;
using LanguageExt;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items;

public sealed class StaticPriceItem : Item
{
    public const Int32 StaticPrice = 2;

    public override IEnumerable<String> Names { get; } = new String[] { "StaticPriceItem", "s" };

    public override Int32 GetPrice(Player player)
    {
        return StaticPrice;
    }

    public override String GetSpecificNameForNotEnoughToBuy(Player player)
    {
        return "a Static Price Item";
    }

    public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
    {
        return quantity == 1 ? "Static Price Item" : "Static Price Items";
    }

    public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
    {
        player.Points -= price;
        return 1;
    }
}
