using System;
using Chubberino.Common.ValueObjects;

namespace Chubberino.Bots.Common.Commands.Settings.ColorSelectors
{
    public interface IColorSelector
    {
        public Name Name { get; }

        String GetNextColor();
    }
}
