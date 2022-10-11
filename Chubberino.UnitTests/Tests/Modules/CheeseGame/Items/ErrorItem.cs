using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items;
using Chubberino.Database.Models;
using LanguageExt;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items;

public sealed class ErrorItem : Item
{
    public const String ErrorMessage = "a";

    public override IEnumerable<String> Names { get; } = new String[]
    {
        "ErrorItem",
        "e"
    };

    public override Either<Int32, String> GetPrice(Player player)
        => ErrorMessage;

    public override String GetSpecificNameForNotEnoughToBuy(Player player)
        => "an Error Item";

    public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        => quantity == 1 ? "Error Item" : "Error Items";

    public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
    {
        player.Points -= price;
        return ErrorMessage;
    }
}
