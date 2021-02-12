using System;

namespace Chubberino.Modules.CheeseGame
{
    public class CheeseVariant
    {
        public CheeseVariant(String name, Int32 pointValue)
        {
            Name = name;
            PointValue = pointValue;
        }

        public String Name { get; }

        public Int32 PointValue { get; }
    }
}
