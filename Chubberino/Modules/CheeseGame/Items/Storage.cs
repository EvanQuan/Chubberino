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

        public override Int32 GetPrice(Player player)
        {
            return 25 + player.MaximumPointStorage / 2;
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            Int32 storageGain = (Int32)(Constants.ShopStorageQuantity * player.GetStorageUpgradeMultiplier());

            return $"{storageGain} storage units";
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            return "storage units";
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
