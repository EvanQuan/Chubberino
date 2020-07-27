using System;

namespace Chubberino.Client.Commands.Settings.Colors
{
    public sealed class RandomColorSelector : IColorSelector
    {
        private Func<String> CurrentColor { get; }

        private Random Random { get; }

        public String Name { get; } = "random";

        public RandomColorSelector(Random random, Func<String> currentColor)
        {
            Random = random;
            CurrentColor = currentColor;
        }

        public String GetNextColor()
        {
            String currentColor = CurrentColor();

            String randomColor;
            do
            {
                randomColor = $"#{Random.Next(0x1000000):X6}";
            }
            while (randomColor == currentColor);

            return randomColor;
        }
    }
}
