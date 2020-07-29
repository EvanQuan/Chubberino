using System;

namespace Chubberino.Client.Commands.Settings.Colors
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

        public String Name { get; } = "rainbow";

        private Int32 ColorIndex { get; set; }

        public String GetNextColor()
        {
            return RainbowColors[ColorIndex++ % RainbowColors.Length];
        }
    }
}
