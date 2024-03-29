﻿using Chubberino.Common.ValueObjects;

namespace Chubberino.Bots.Common.Commands.Settings.ColorSelectors;

public sealed class RandomColorSelector : IColorSelector
{
    private Random Random { get; }

    public Name Name { get; } = "random";

    public RandomColorSelector(Random random)
    {
        Random = random;
    }

    public String GetNextColor()
        => $"#{Random.Next(0x1000000):X6}";
}
