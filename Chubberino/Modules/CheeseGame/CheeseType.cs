using System;

namespace Chubberino.Modules.CheeseGame
{
    public sealed class CheeseType
    {
        public CheeseType(String name, Int32 pointValue)
        {
            Name = name;
            PointValue = pointValue;
        }

        public String Name { get; }

        public Int32 PointValue { get; }
    }
}
