using System;

namespace Chubberino.Modules.CheeseGame.Items
{
    public class PriceList
    {
        public PriceList(
            Int32 storage,
            Int32 population,
            Int32 worker,
            Int32 mousetrap)
        {
            Storage = storage;
            Population = population;
            Worker = worker;
            MouseTrap = mousetrap;
        }

        public Int32 Storage { get; }

        public Int32 Population { get; }

        public Int32 Worker { get; }

        public Int32 MouseTrap { get; }
    }
}
