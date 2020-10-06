using System;

namespace Chubberino.Client.Commands.Settings.ColorSelectors
{
    public interface IColorSelector
    {
        public String Name { get; }

        String GetNextColor();
    }
}
