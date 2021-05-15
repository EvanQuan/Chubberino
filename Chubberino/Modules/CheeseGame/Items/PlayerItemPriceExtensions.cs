using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Utility;
using System;

namespace Chubberino.Modules.CheeseGame.Items
{
    public static class PlayerItemPriceExtensions
    {
        public static Int32 GetGearPrice(this Player player)
        {
            // Every 20 gear (every 10%), the increment increases by 1.
            Int32 incrementGroups = 0.Max(player.GearCount / 20);

            Int32 remainder = player.GearCount % 20 + 1;

            Int32 price = 1;

            // Add increments for each full group of 20.
            for (Int32 increment = 0; increment < incrementGroups; increment++)
            {
                price += increment * 20;
            }

            // Add final increments for remainder.
            price += incrementGroups * remainder;

            return price;
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
