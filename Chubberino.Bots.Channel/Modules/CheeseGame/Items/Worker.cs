using System.Collections.Generic;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items;

public sealed class Worker : Item
{
    public const String NotEnoughPopulationMessage = "You do not have enough population slots for another worker. Consider buying more population with \"!cheese buy population\".";

    public override IEnumerable<String> Names { get; } = new String[]
    {
        "Worker",
        "w",
        "workers"
    };

    public override Either<Int32, String> GetPrice(Player player)
        => player.GetWorkerPrice();

    public override String GetSpecificNameForNotEnoughToBuy(Player player)
        => "a worker";

    public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        => quantity + (quantity == 1 ? " worker" : " workers");

    public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
    {
        if (player.WorkerCount < player.PopulationCount)
        {
            player.WorkerCount++;
            player.Points -= price;
            return 1;
        }
        else
        {
            return NotEnoughPopulationMessage;
        }
    }

    public override Option<String> GetShopPrompt(Player player)
        => $"{base.GetShopPrompt(player)} [+1] for {GetPrice(player)}";
}
