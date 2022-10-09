namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.DisableAlls;

/// <summary>
/// When all the settings are already disabled.
/// </summary>
public sealed class WhenAllSettingsAreDisabled : UsingDisableAll
{

    /// <summary>
    /// Should not change any of the <see cref="ISetting.IsEnabled"/> values.
    /// </summary>
    [Fact]
    public void ShouldDisableAllSettingsWithNoArguments()
    {
        Sut.Execute(Array.Empty<String>());

        MockedCommandRepository.Verify(x => x.DisableAllSettings(), Times.Once());
    }

    [Fact]
    public void ShouldDisableAllSettingsWithGarbageArguments()
    {
        Sut.Execute(new String[] { "asdf", "38f9f" });

        MockedCommandRepository.Verify(x => x.DisableAllSettings(), Times.Once());
    }
}
