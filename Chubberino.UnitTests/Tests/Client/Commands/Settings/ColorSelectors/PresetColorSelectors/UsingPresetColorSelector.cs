using Chubberino.Bots.Common.Commands.Settings.ColorSelectors;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.ColorSelectors.PresetColorSelectors;

public abstract class UsingPresetColorSelector
{
    protected PresetColorSelector Sut { get; }

    protected Mock<Random> MockedRandom { get; }

    public UsingPresetColorSelector()
    {
        MockedRandom = new Mock<Random>().SetupAllProperties();

        Sut = new PresetColorSelector(MockedRandom.Object);
    }
}
