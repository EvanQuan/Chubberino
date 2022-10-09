using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items;
using Chubberino.Database.Models;
using LanguageExt;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items;

public sealed class IncrementingQuantityItem : Item
{
    public const Int32 StaticPrice = 1;

    public const Int32 InitialQuantity = 2;

    public Int32 Quantity { get; private set; } = InitialQuantity;

    public override IEnumerable<String> Names { get; } = new String[] { "IncrementingQuantityItem", "q" };

    public override Int32 GetPrice(Player player)
    {
        return StaticPrice;
    }

    public override String GetSpecificNameForNotEnoughToBuy(Player player)
    {
        return "a Multiple Quantity Item";
    }

    public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
    {
        return quantity == 1 ? "Multiple Quantity Item" : "Multiple Quantity Items";
    }

    public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
    {
        player.Points -= price;
        return Quantity++;
    }
}
