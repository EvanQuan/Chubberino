using System.Collections.Generic;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items;

public sealed class Population : Item
{
    public const Int32 ShopUnitQuantity = 5;

    public override IEnumerable<String> Names { get; } = new String[] { "Population", "p", "pop" };

    public override Either<Int32, String> GetPrice(Player player)
        => (Int32)(20 + Math.Pow(player.PopulationCount, 2));

    public override String GetSpecificNameForNotEnoughToBuy(Player player)
        => ShopUnitQuantity + " population slots";

    public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        => quantity + " population slots";

    public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
    {
        player.PopulationCount += ShopUnitQuantity;
        player.Points -= price;

        return ShopUnitQuantity;
    }

    public override Option<String> GetShopPrompt(Player player)
        => $"{GetBaseShopPrompt(player)} [+{ShopUnitQuantity}] for {GetPriceString(player)}";
}
