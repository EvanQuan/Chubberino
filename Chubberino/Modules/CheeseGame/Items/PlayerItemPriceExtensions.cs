using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Items
{
    public static class PlayerItemPriceExtensions
    {
        public static Int32 GetGearPrice(this Player player)
        {
            return 1 + player.GearCount / 20 * player.GearCount;
        }

        public static Int32 GetMousetrapPrice(this Player player)
        {
            return 25;
        }

        public static Int32 GetStoragePrice(this Player player)
        {
            return 25 + player.MaximumPointStorage / 2;
        }

        public static Int32 GetWorkerPrice(this Player player)
        {
            return (Int32)(100 + 10 * Math.Pow(player.WorkerCount, 1.4));
        }
    }
}
