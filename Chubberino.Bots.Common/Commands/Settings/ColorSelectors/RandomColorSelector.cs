using System;
using Chubberino.Bots.Common.Commands.Settings.ColorSelectors;
using Chubberino.Common.ValueObjects;

namespace Chubberino.Client.Commands.Settings.ColorSelectors
{
    public sealed class RandomColorSelector : IColorSelector
    {
        private Random Random { get; }

        public LowercaseString Name { get; } = LowercaseString.From("random");

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
