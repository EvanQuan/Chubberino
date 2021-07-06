using System;
using Chubberino.Bots.Common.Commands.Settings.ColorSelectors;
using Chubberino.Common.ValueObjects;
using TwitchLib.Api.Core.Enums;

namespace Chubberino.Client.Commands.Settings.ColorSelectors
{
    public sealed class PresetColorSelector : IColorSelector
    {
        private static Array ColorPresets { get; } = Enum.GetValues(typeof(ChatColorPresets));

        private Random Random { get; }

        public LowercaseString Name { get; } = LowercaseString.From("preset");

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
