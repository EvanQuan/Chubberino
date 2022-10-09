namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories;

public sealed class WhenAllSettingsAreDisabled : UsingCommandRepository
{
    public WhenAllSettingsAreDisabled()
    {
        Sut
            .AddCommand(MockedSetting1.Object)
            .AddCommand(MockedSetting2.Object)
            .AddCommand(MockedSetting3.Object)
            .AddCommand(MockedCommand.Object);
    }
    /// <summary>
    /// All the settings are already disabled, so there is nothing to do.
    /// </summary>
    [Fact]
    public void ShouldDoNothing()
    {
        Sut.DisableAllSettings();

        Assert.Empty(Sut.Settings.Enabled);
        Assert.Contains(MockedSetting1.Object, Sut.Settings.Disabled);
        Assert.Contains(MockedSetting2.Object, Sut.Settings.Disabled);
        Assert.Contains(MockedSetting3.Object, Sut.Settings.Disabled);
    }
}
