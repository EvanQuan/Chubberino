using System;
using Chubberino.Infrastructure.Commands;
using FluentAssertions;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories;

public sealed class WhenGettingStatus : UsingCommandRepository
{
    public WhenGettingStatus()
    {
        Sut
            .AddCommand(MockedSetting2.Object)
            .AddCommand(MockedSetting1.Object)
            .AddCommand(MockedSetting3.Object)
            .AddCommand(MockedCommand.Object);
    }

    [Fact]
    public void ShouldOrder1DisabledOver2EnabledSettings()
    {
        Sut.Settings.Disable(MockedSetting1.Object.Name);
        Sut.Settings.Enable(MockedSetting2.Object.Name);
        Sut.Settings.Enable(MockedSetting3.Object.Name);

        String expectedStatus =
            CommandRepository.StatusLine + "Commands" + CommandRepository.StatusLine + Environment.NewLine +
            MockedCommand.Object.Name + Environment.NewLine +
            CommandRepository.StatusLine + "User Commands" + CommandRepository.StatusLine + Environment.NewLine +
            CommandRepository.StatusLine + "Disabled Settings" + CommandRepository.StatusLine + Environment.NewLine +
            MockedSetting1.Object.Name + ": " + MockedSetting1.Object.Status + Environment.NewLine +
            CommandRepository.StatusLine + "Enabled Settings" + CommandRepository.StatusLine + Environment.NewLine +
            MockedSetting2.Object.Name + ": " + MockedSetting2.Object.Status + Environment.NewLine +
            MockedSetting3.Object.Name + ": " + MockedSetting3.Object.Status + Environment.NewLine;

        var status = Sut.GetStatus();

        status.Should().Be(expectedStatus);
    }

    [Fact]
    public void ShouldOrder2DisabledOver1EnabledSettings()
    {
        Sut.Settings.Disable(MockedSetting1.Object.Name);
        Sut.Settings.Disable(MockedSetting2.Object.Name);
        Sut.Settings.Enable(MockedSetting3.Object.Name);

        String expectedStatus =
            CommandRepository.StatusLine + "Commands" + CommandRepository.StatusLine + Environment.NewLine +
            MockedCommand.Object.Name + Environment.NewLine +
            CommandRepository.StatusLine + "User Commands" + CommandRepository.StatusLine + Environment.NewLine +
            CommandRepository.StatusLine + "Disabled Settings" + CommandRepository.StatusLine + Environment.NewLine +
            MockedSetting1.Object.Name + ": " + MockedSetting1.Object.Status + Environment.NewLine +
            MockedSetting2.Object.Name + ": " + MockedSetting2.Object.Status + Environment.NewLine +
            CommandRepository.StatusLine + "Enabled Settings" + CommandRepository.StatusLine + Environment.NewLine +
            MockedSetting3.Object.Name + ": " + MockedSetting3.Object.Status + Environment.NewLine;

        var status = Sut.GetStatus();

        status.Should().Be(expectedStatus);
    }

    [Fact]
    public void ShouldOrder3DisabledOver0EnabledSettings()
    {
        Sut.Settings.Disable(MockedSetting1.Object.Name);
        Sut.Settings.Disable(MockedSetting2.Object.Name);
        Sut.Settings.Disable(MockedSetting3.Object.Name);

        String expectedStatus =
            CommandRepository.StatusLine + "Commands" + CommandRepository.StatusLine + Environment.NewLine +
            MockedCommand.Object.Name + Environment.NewLine +
            CommandRepository.StatusLine + "User Commands" + CommandRepository.StatusLine + Environment.NewLine +
            CommandRepository.StatusLine + "Disabled Settings" + CommandRepository.StatusLine + Environment.NewLine +
            MockedSetting1.Object.Name + ": " + MockedSetting1.Object.Status + Environment.NewLine +
            MockedSetting2.Object.Name + ": " + MockedSetting2.Object.Status + Environment.NewLine +
            MockedSetting3.Object.Name + ": " + MockedSetting3.Object.Status + Environment.NewLine +
            CommandRepository.StatusLine + "Enabled Settings" + CommandRepository.StatusLine + Environment.NewLine;

        var status = Sut.GetStatus();

        status.Should().Be(expectedStatus);
    }

    [Fact]
    public void ShouldOrder0DisabledOver3EnabledSettings()
    {
        Sut.Settings.Enable(MockedSetting1.Object.Name);
        Sut.Settings.Enable(MockedSetting2.Object.Name);
        Sut.Settings.Enable(MockedSetting3.Object.Name);

        String expectedStatus =
            CommandRepository.StatusLine + "Commands" + CommandRepository.StatusLine + Environment.NewLine +
            MockedCommand.Object.Name + Environment.NewLine +
            CommandRepository.StatusLine + "User Commands" + CommandRepository.StatusLine + Environment.NewLine +
            CommandRepository.StatusLine + "Disabled Settings" + CommandRepository.StatusLine + Environment.NewLine +
            CommandRepository.StatusLine + "Enabled Settings" + CommandRepository.StatusLine + Environment.NewLine +
            MockedSetting1.Object.Name + ": " + MockedSetting1.Object.Status + Environment.NewLine +
            MockedSetting2.Object.Name + ": " + MockedSetting2.Object.Status + Environment.NewLine +
            MockedSetting3.Object.Name + ": " + MockedSetting3.Object.Status + Environment.NewLine;

        var status = Sut.GetStatus();

        status.Should().Be(expectedStatus);
    }
}
