namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Colors;

public sealed class WhenAddingColorSelectors : UsingColor
{
    [Fact]
    public void ShouldAddColorSelector()
    {
        Sut.AddColorSelector(MockedSelector1.Object);

        Assert.Single(Sut.Selectors);

        Assert.Equal(MockedSelector1.Object, Sut.Selectors[0]);
    }
}
