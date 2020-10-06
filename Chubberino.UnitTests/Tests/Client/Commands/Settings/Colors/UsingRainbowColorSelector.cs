using Chubberino.Client.Commands.Settings.Colors;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Colors
{
    public abstract class UsingRainbowColorSelector
    {
        protected RainbowColorSelector Sut { get; }

        public UsingRainbowColorSelector()
        {
            Sut = new RainbowColorSelector();
        }
    }
}
