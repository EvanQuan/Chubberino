using Chubberino.Bots.Common.Commands.Settings.ColorSelectors;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.ColorSelectors.RainbowColorSelectors;

public abstract class UsingRainbowColorSelector
{
    protected RainbowColorSelector Sut { get; }

    public UsingRainbowColorSelector()
    {
        Sut = new RainbowColorSelector();
    }
}
