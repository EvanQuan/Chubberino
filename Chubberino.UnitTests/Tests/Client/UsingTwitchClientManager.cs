using System.IO;
using Chubberino.Common.Services;
using Chubberino.Common.ValueObjects;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Configurations;
using Chubberino.Infrastructure.Credentials;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.UnitTests.Tests.Client;

public abstract class UsingTwitchClientManager
{
    protected TwitchClientManager Sut { get; }

    protected Mock<IBot> MockedBot { get; } = new();

    protected Mock<IApplicationContextFactory> MockedApplicationContextFactory { get; } = new();
    protected Mock<IConfig> MockedConfig { get; } = new();

    protected Mock<IApplicationContext> MockedApplicationContext { get; } = new();

    protected Mock<ITwitchClientFactory> MockedTwitchClientFactory { get; } = new();

    protected Mock<ITwitchClient> MockedTwitchClient { get; } = new();

    protected Mock<ICredentialsManager> MockedCredentialsManager { get; } = new();

    protected Mock<ISpinWaitService> MockedSpinWait { get; } = new();

    protected Mock<IThreadService> MockedThreadService { get; } = new();

    protected Mock<IDateTimeService> MockedDateTimeService { get; } = new();

    protected ApplicationCredentials ApplicationCredentials { get; }

    protected LoginCredentials LoginCredentials { get; }

    protected ConnectionCredentials ConnectionCredentials { get; }

    protected Mock<TextWriter> MockedWriter { get; }

    protected String TwitchUsername { get; }

    protected String TwitchOAuth { get; }

    protected Name PrimaryChannelName { get; }

    protected UsingTwitchClientManager()
    {
        TwitchUsername = Guid.NewGuid().ToString();
        TwitchOAuth = Guid.NewGuid().ToString();

        PrimaryChannelName = Name.From("p");

        MockedBot = new();
        ApplicationCredentials = new();
        ConnectionCredentials = new(TwitchUsername, TwitchOAuth, disableUsernameCheck: true);
        LoginCredentials = new LoginCredentials(ConnectionCredentials, true, PrimaryChannelName);
        MockedWriter = new();

        Sut = new TwitchClientManager(
            MockedConfig.Object,
            MockedTwitchClientFactory.Object,
            MockedCredentialsManager.Object,
            MockedSpinWait.Object,
            MockedThreadService.Object,
            MockedDateTimeService.Object,
            MockedWriter.Object);

        MockedTwitchClientFactory
            .Setup(x => x.CreateClient(It.IsAny<IClientOptions>()))
            .Returns(MockedTwitchClient.Object);

        MockedCredentialsManager
            .Setup(x => x.TryUpdateLoginCredentials(It.IsAny<LoginCredentials>()))
            .Returns(LoginCredentials);

        MockedCredentialsManager
            .Setup(x => x.ApplicationCredentials)
            .Returns(ApplicationCredentials);

        MockedApplicationContextFactory
            .Setup(x => x.GetContext())
            .Returns(MockedApplicationContext.Object);
    }
}
