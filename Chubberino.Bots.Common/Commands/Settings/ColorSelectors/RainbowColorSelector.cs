using System;
using Chubberino.Bots.Common.Commands.Settings.ColorSelectors;
using Chubberino.Common.ValueObjects;

namespace Chubberino.Client.Commands.Settings.ColorSelectors
{
    public sealed class RainbowColorSelector : IColorSelector
    {
        private static String[] RainbowColors { get; } = new String[]
        {
            "#FF0000",
            "#FF7F00",
            "#FFFF00",
            "#00FF00",
            "#0000FF",
            "#4B0082",
            "#9400D3",
        };

        public Name Name { get; } = Name.From("rainbow");

        private Int32 ColorIndex { get; set; }

        public String GetNextColor()
        {
            ColorIndex = (ColorIndex + 1) % RainbowColors.Length;

            return RainbowColors[ColorIndex];
        }
    }
}
