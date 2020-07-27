using System;

namespace Chubberino.Client.Commands.Settings.Colors
{
    public interface IColorSelector
    {
        public String Name { get; }

        String GetNextColor();
    }
}
