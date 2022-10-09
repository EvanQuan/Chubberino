namespace Chubberino.UnitTests.Tests.Client.Commands.DisableAlls;

public sealed class WhenExecuting : UsingDisableAll
{
    [Theory]
    [InlineData("a")]
    [InlineData("a", "b")]
    public void ShouldDisableAllSettings(params String[] arguments)
    {
        Sut.Execute(arguments);

        MockedCommandRepository.Verify(x => x.DisableAllSettings(), Times.Once());

        MockedWriter.Verify(x => x.WriteLine("Disabled all settings."), Times.Once());
    }

    [Fact]
    public void ShouldDisableAllSettingsWithNoArguments()
    {
        Sut.Execute(Array.Empty<String>());

        MockedCommandRepository.Verify(x => x.DisableAllSettings(), Times.Once());

        MockedWriter.Verify(x => x.WriteLine("Disabled all settings."), Times.Once());
    }
}
