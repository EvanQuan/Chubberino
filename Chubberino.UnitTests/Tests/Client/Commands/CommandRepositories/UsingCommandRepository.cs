using System.IO;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;
using Chubberino.Infrastructure.Commands.Settings;
using Chubberino.Infrastructure.Commands.Settings.UserCommands;
using Chubberino.UnitTestQualityTools.Extensions;
using TwitchLib.Client.Interfaces;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories;

public abstract class UsingCommandRepository
{
    /// <summary>
    /// System under test.
    /// </summary>
    protected CommandRepository Sut { get; }

    protected Mock<TextWriter> MockedWriter { get; }

    protected Mock<ITwitchClientManager> MockedTwitchClientManager { get; }

    protected Mock<ITwitchClient> MockedTwitchClient { get; }

    protected Mock<ISetting> MockedSetting1 { get; }

    protected Mock<ISetting> MockedSetting2 { get; }

    protected Mock<ISetting> MockedSetting3 { get; }

    protected Mock<ICommand> MockedCommand { get; }

    protected Mock<IUserCommand> MockedUserCommand1 { get; }

    protected Mock<IUserCommand> MockedUserCommand2 { get; }

    protected Mock<IUserCommandValidator> MockedUserCommandValidator { get; }
    public Mock<ICommandConfigurationStrategy> MockedCommandConfigurationStrategy { get; set; }

    public UsingCommandRepository()
    {
        MockedWriter = new();

        MockedTwitchClientManager = new();

        MockedUserCommandValidator = new();

        MockedCommandConfigurationStrategy = new();

        MockedTwitchClientManager.SetupAllProperties();

        MockedTwitchClient = MockedTwitchClientManager.SetupClient();

        MockedSetting1 = new();
        MockedSetting2 = new();
        MockedSetting3 = new();
        MockedCommand = new();
        MockedUserCommand1 = new();
        MockedUserCommand2 = new();

        MockedSetting1.SetupAllProperties();
        MockedSetting2.SetupAllProperties();
        MockedSetting3.SetupAllProperties();
        MockedCommand.SetupAllProperties();
        MockedUserCommand1.SetupAllProperties();
        MockedUserCommand2.SetupAllProperties();

        MockedSetting1.Setup(x => x.Name).Returns("s1");
        MockedSetting2.Setup(x => x.Name).Returns("s2");
        MockedSetting3.Setup(x => x.Name).Returns("s3");
        MockedCommand.Setup(x => x.Name).Returns("c");
        MockedUserCommand1.Setup(x => x.Name).Returns("uc1");
        MockedUserCommand2.Setup(x => x.Name).Returns("uc2");

        MockedSetting1.Setup(x => x.Status).Returns("1");
        MockedSetting2.Setup(x => x.Status).Returns("2");
        MockedSetting3.Setup(x => x.Status).Returns("3");

        Sut = new CommandRepository(
            MockedWriter.Object,
            MockedTwitchClientManager.Object,
            MockedUserCommandValidator.Object,
            MockedCommandConfigurationStrategy.Object);
    }
}
