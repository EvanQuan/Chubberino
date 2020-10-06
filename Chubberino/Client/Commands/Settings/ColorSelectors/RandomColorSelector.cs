using System;

namespace Chubberino.Client.Commands.Settings.ColorSelectors
{
    public sealed class RandomColorSelector : IColorSelector
    {
        private Random Random { get; }

        public String Name { get; } = "random";

        public RandomColorSelector(Random random)
        {
            Random = random;
        }

        public String GetNextColor()
        {
            return $"#{Random.Next(0x1000000):X6}";
        }
    }
}
