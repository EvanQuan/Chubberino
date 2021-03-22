using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class CheeseModifier
    {
        public CheeseModifier(String name, Int32 points)
        {
            Name = name;
            Points = points;
        }

        public String Name { get; }
        public Int32 Points { get; }
    }
}
