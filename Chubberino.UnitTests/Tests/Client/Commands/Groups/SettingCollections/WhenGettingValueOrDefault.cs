using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Groups.SettingCollections;

public sealed class WhenGettingValueOrDefault : UsingSettingCollection
{
    [Fact]
    public void ShouldReturnEnabledCommand()
    {
        Sut.AddEnabled(Element1.Object);
        Sut.AddEnabled(Element2.Object);
        Sut.AddDisabled(Element3.Object);

        var result = Sut.GetValueOrDefault(Element1.Object.Name);
        Assert.Equal(Element1.Object, result);
    }

    [Fact]
    public void ShouldReturnDisabledCommand()
    {
        Sut.AddDisabled(Element1.Object);
        Sut.AddEnabled(Element2.Object);
        Sut.AddDisabled(Element3.Object);

        var result = Sut.GetValueOrDefault(Element1.Object.Name);
        Assert.Equal(Element1.Object, result);
    }

    [Fact]
    public void ShouldReturnDefault()
    {
        Sut.AddEnabled(Element2.Object);
        Sut.AddDisabled(Element3.Object);

        var result = Sut.GetValueOrDefault(Element1.Object.Name);
        Assert.Null(result);
    }
}
