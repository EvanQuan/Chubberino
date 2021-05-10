using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Items
{
    public sealed class Storage : Item
    {
        public override IEnumerable<String> Names { get; } = new String[] { "Storage", "s" };

        public override Int32 GetPriceFunctionDeterminer(Player player)
        {
            return player.MaximumPointStorage;
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            Int32 storageGain = (Int32)(Constants.ShopStorageQuantity * player.GetStorageUpgradeMultiplier());

            return $"{storageGain} storage";
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            return "storage";
        }

        public override Int32 PriceFunction(Int32 priceDeterminer)
        {
            return 25 + priceDeterminer / 2;
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            player.MaximumPointStorage += Constants.ShopStorageQuantity;
            player.Points -= price;

            Int32 storageGain = (Int32)(Constants.ShopStorageQuantity * player.GetStorageUpgradeMultiplier());

            return () => storageGain;
        }
    }
}
