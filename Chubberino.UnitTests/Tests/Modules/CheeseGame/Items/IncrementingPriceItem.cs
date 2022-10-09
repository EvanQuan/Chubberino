using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items;
using Chubberino.Database.Models;
using LanguageExt;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items;

public sealed class IncrementingPriceItem : Item
{
    public const Int32 InitialPrice = 2;

    public Int32 Price { get; private set; } = InitialPrice;

    public override IEnumerable<String> Names { get; } = new String[] { "IncrementingPriceItem", "s" };

    public override Int32 GetPrice(Player player)
    {
        return Price++;
    }

    public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
    {
        player.Points -= price;
        return 1;
    }

    public override String GetSpecificNameForNotEnoughToBuy(Player player)
    {
        return "an Incrementing Price Item";
    }


    public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
    {
        return quantity == 1 ? "Incrementing Price Item" : "Incrementing Price Items";
    }
}
