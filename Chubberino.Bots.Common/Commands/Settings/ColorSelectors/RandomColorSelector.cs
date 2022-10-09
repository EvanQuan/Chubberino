using Chubberino.Common.ValueObjects;

namespace Chubberino.Bots.Common.Commands.Settings.ColorSelectors;

public sealed class RandomColorSelector : IColorSelector
{
    private Random Random { get; }

    public Name Name { get; } = Name.From("random");

    public RandomColorSelector(Random random)
    {
        Random = random;
    }

    public String GetNextColor()
    {
        return $"#{Random.Next(0x1000000):X6}";
    }
}
