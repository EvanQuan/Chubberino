using System;

namespace Chubberino.Client.Commands.Settings.Colors
{
    public sealed class RainbowColorSelector : IColorSelector
    {
        private static String[] RainbowColors { get; } = new String[]
        {
            "#9400D3",
            "#4B0082",
            "#0000FF",
            "#00FF00",
            "#FFFF00",
            "#FF7F00",
            "#FF0000",
        };

        public String Name { get; } = "rainbow";

        private Int32 ColorIndex { get; set; }

        public String GetNextColor()
        {
            return RainbowColors[ColorIndex++ % RainbowColors.Length];
        }
    }
}
