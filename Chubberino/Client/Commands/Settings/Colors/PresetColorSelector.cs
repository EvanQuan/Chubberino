using System;
using TwitchLib.Api.Core.Enums;

namespace Chubberino.Client.Commands.Settings.Colors
{
    public sealed class PresetColorSelector : IColorSelector
    {
        private static Array ColorPresets { get; } = Enum.GetValues(typeof(ChatColorPresets));

        private Random Random { get; }

        public String Name { get; } = "preset";

        public PresetColorSelector(Random random)
        {
            Random = random;
        }

        public String GetNextColor()
        {
            return ColorPresets.GetValue(Random.Next(ColorPresets.Length)).ToString();
        }
    }
}
