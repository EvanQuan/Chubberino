using System;
using Chubberino.Common.ValueObjects;

namespace Chubberino.Bots.Common.Commands.Settings.ColorSelectors
{
    public interface IColorSelector
    {
        public LowercaseString Name { get; }

        String GetNextColor();
    }
}
