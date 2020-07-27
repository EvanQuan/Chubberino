using System;
using TwitchLib.Api.Core.Enums;

namespace Chubberino.Client.Commands.Settings.Colors
{
    public sealed class PresetColorSelector : IColorSelector
    {
        private static Array ColorPresets { get; } = Enum.GetValues(typeof(ChatColorPresets));

        private Func<String> CurrentColor { get; }

        private Random Random { get; }

        public String Name { get; } = "preset";

        public PresetColorSelector(Random random, Func<String> currentColor)
        {
            Random = random;
            CurrentColor = currentColor;
        }

        public String GetNextColor()
        {
            String currentColor = CurrentColor();
            String presetColor;
            do
            {
                presetColor = ColorPresets.GetValue(Random.Next(ColorPresets.Length)).ToString();
            }
            while (presetColor == currentColor);

            return presetColor;
        }
    }
}
