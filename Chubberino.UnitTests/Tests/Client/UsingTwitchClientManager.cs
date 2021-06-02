using Chubberino.Client;
using Chubberino.Client.Credentials;
using Chubberino.Client.Services;
using Chubberino.Client.Threading;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Moq;
using System;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.UnitTests.Tests.Client
{
    public abstract class UsingTwitchClientManager
    {
        protected TwitchClientManager Sut { get; }

        protected Mock<IBot> MockedBot { get; }

        protected Mock<IApplicationContextFactory> MockedApplicationContextFactory { get; }

        protected Mock<IApplicationContext> MockedApplicationContext { get; }

        protected Mock<ITwitchClientFactory> MockedTwitchClientFactory { get; }

        protected Mock<ITwitchClient> MockedTwitchClient { get; }

        protected Mock<ICredentialsManager> MockedCredentialsManager { get; }

        protected Mock<ISpinWait> MockedSpinWait { get; }

        protected Mock<IDateTimeService> MockedDateTimeService { get; }

        protected ApplicationCredentials ApplicationCredentials { get; }

        protected LoginCredentials LoginCredentials { get; }

        protected ConnectionCredentials ConnectionCredentials { get; }

        protected Mock<IConsole> MockedConsole { get; }

        protected String TwitchUsername { get; }

        protected String TwitchOAuth { get; }

        protected UsingTwitchClientManager()
        {
            TwitchUsername = Guid.NewGuid().ToString();
            TwitchOAuth = Guid.NewGuid().ToString();

            MockedBot = new();
            MockedApplicationContextFactory = new();
            MockedApplicationContext = new();
            MockedTwitchClientFactory = new();
            MockedTwitchClient = new();
            MockedCredentialsManager = new();
            MockedSpinWait = new();
            MockedDateTimeService = new();
            ApplicationCredentials = new();
            ConnectionCredentials = new(TwitchUsername, TwitchOAuth, disableUsernameCheck: true);
            LoginCredentials = new(ConnectionCredentials, true);
            MockedConsole = new();

            Sut = new TwitchClientManager(
                MockedApplicationContextFactory.Object,
                MockedTwitchClientFactory.Object,
                MockedCredentialsManager.Object,
                MockedSpinWait.Object,
                MockedDateTimeService.Object,
                MockedConsole.Object);

            MockedTwitchClientFactory
                .Setup(x => x.GetClient(It.IsAny<IClientOptions>()))
                .Returns(MockedTwitchClient.Object);

            var credentials = LoginCredentials;
            MockedCredentialsManager
                .Setup(x => x.TryGetCredentials(out credentials))
                .Returns(true);

            MockedCredentialsManager
                .Setup(x => x.ApplicationCredentials)
                .Returns(ApplicationCredentials);

            MockedApplicationContextFactory
                .Setup(x => x.GetContext())
                .Returns(MockedApplicationContext.Object);
        }
    }
}
