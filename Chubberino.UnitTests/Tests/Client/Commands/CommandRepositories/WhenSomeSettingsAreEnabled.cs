namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories;

public sealed class WhenSomeSettingsAreEnabled : UsingCommandRepository
{
    /// <summary>
    /// The settings are were enabled should be disabled, and the disabled
    /// commands should remain disabled.
    /// </summary>
    [Fact]
    public void ShouldDisableAllSettings()
    {
        Sut.AddCommand(MockedSetting1.Object);
        Sut.AddCommand(MockedCommand.Object);
        Sut.AddCommand(MockedSetting2.Object);

        Sut.Settings.Enable(MockedSetting1.Object.Name);

        Sut.DisableAllSettings();

        Assert.Empty(Sut.Settings.Enabled);
        Assert.Contains(MockedSetting1.Object, Sut.Settings.Disabled);
        Assert.Contains(MockedSetting2.Object, Sut.Settings.Disabled);
    }
}
