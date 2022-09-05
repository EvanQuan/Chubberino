using Chubberino.Common.ValueObjects;
using Chubberino.Infrastructure.Commands.Settings;
using Moq;

namespace Chubberino.UnitTests.Tests.Client.Commands.Groups.SettingCollections;

public abstract class UsingSettingCollection
{
    protected SettingCollection<ISetting> Sut { get; }

    protected Mock<ISetting> Element1 { get; }

    protected Mock<ISetting> Element2 { get; }

    protected Mock<ISetting> Element3 { get; }

    protected UsingSettingCollection()
    {
        Element1 = new();
        Element2 = new();
        Element3 = new();

        Element1.SetupAllProperties();
        Element2.SetupAllProperties();
        Element3.SetupAllProperties();

        Sut = new();

        Element1.Setup(x => x.Name).Returns(Name.From("1"));
        Element2.Setup(x => x.Name).Returns(Name.From("2"));
        Element3.Setup(x => x.Name).Returns(Name.From("3"));
    }
}
