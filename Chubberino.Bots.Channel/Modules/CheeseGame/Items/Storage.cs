using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Storages;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items;

public sealed class Storage : Item
{
    /// <summary>
    /// Base storage quantity gained when buying from the shop.
    /// </summary>
    public const Int32 BaseQuantity = 100;

    public override IEnumerable<String> Names { get; } = new String[] { "Storage", "s" };

    public override Either<Int32, String> GetPrice(Player player)
        => player.GetStoragePrice();

    public override String GetSpecificNameForNotEnoughToBuy(Player player)
    {
        Int32 storageGain = (Int32)(BaseQuantity * player.GetStorageUpgradeMultiplier());

        return $"{storageGain} storage units";
    }

    public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity) => $"{quantity} storage units";

    public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
    {
        player.MaximumPointStorage += BaseQuantity;
        player.Points -= price;

        Int32 storageGain = (Int32)(BaseQuantity * player.GetStorageUpgradeMultiplier());

        return storageGain;
    }

    public override Option<String> GetShopPrompt(Player player)
    {
        Int32 storageGain = (Int32)(BaseQuantity * player.GetStorageUpgradeMultiplier());

        return $"{GetBaseShopPrompt(player)} [+{storageGain}] for {GetPriceString(player)} cheese";
    }
}
