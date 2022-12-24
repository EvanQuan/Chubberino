using Chubberino.Common.ValueObjects;
using TwitchLib.Client.Enums;

namespace Chubberino.Bots.Common.Commands.Settings.ColorSelectors;

public sealed class PresetColorSelector : IColorSelector
{
    private static Array ColorPresets { get; } = Enum.GetValues(typeof(ChatColorPresets));

    private Random Random { get; }

    public Name Name { get; } = "preset";

    public PresetColorSelector(Random random)
    {
        Random = random;
    }

    public String GetNextColor()
        => ColorPresets.GetValue(Random.Next(ColorPresets.Length)).ToString();
}
