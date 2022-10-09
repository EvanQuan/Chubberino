namespace Chubberino.UnitTests.Tests.Client.Commands.Groups.SettingCollections;

public sealed class WhenEnabling : UsingSettingCollection
{
    [Fact]
    public void ShouldEnableExistingDisabledElement()
    {
        Sut.AddDisabled(Element1.Object);

        Sut.Enable(Element1.Object.Name);

        Assert.Single(Sut.Enabled);
        Assert.Contains(Element1.Object, Sut.Enabled);
        Assert.DoesNotContain(Element1.Object, Sut.Disabled);
    }

    [Fact]
    public void ShouldDoNothingOnNonExistingElement()
    {
        Sut.Enable(Element1.Object.Name);

        Assert.DoesNotContain(Element1.Object, Sut.Enabled);
        Assert.DoesNotContain(Element1.Object, Sut.Disabled);
    }

    [Fact]
    public void ShouldDoNothingOnEnabledElement()
    {
        Sut.AddEnabled(Element1.Object);

        Sut.Enable(Element1.Object.Name);

        Assert.Single(Sut.Enabled);
        Assert.Contains(Element1.Object, Sut.Enabled);
        Assert.DoesNotContain(Element1.Object, Sut.Disabled);
    }
}
