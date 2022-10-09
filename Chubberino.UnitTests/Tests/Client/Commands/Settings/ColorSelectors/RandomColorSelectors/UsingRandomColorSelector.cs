using Chubberino.Bots.Common.Commands.Settings.ColorSelectors;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.ColorSelectors.RandomColorSelectors;

public abstract class UsingRandomColorSelector
{
    protected RandomColorSelector Sut { get; }

    protected Mock<Random> MockedRandom { get; }

    public UsingRandomColorSelector()
    {
        MockedRandom = new Mock<Random>().SetupAllProperties();

        Sut = new RandomColorSelector(MockedRandom.Object);
    }

}
