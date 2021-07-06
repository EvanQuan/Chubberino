using System;
using System.IO;
using Chubberino.Common.Services;
using Chubberino.Common.ValueObjects;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Credentials;
using Moq;
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

        protected Mock<ISpinWaitService> MockedSpinWait { get; }

        protected Mock<IThreadService> MockedThreadService { get; }

        protected Mock<IDateTimeService> MockedDateTimeService { get; }

        protected ApplicationCredentials ApplicationCredentials { get; }

        protected LoginCredentials LoginCredentials { get; }

        protected ConnectionCredentials ConnectionCredentials { get; }

        protected Mock<TextWriter> MockedWriter { get; }

        protected String TwitchUsername { get; }

        protected String TwitchOAuth { get; }

        protected LowercaseString PrimaryChannelName { get; }

        protected UsingTwitchClientManager()
        {
            TwitchUsername = Guid.NewGuid().ToString();
            TwitchOAuth = Guid.NewGuid().ToString();

            PrimaryChannelName = LowercaseString.From("p");

            MockedBot = new();
            MockedApplicationContextFactory = new();
            MockedApplicationContext = new();
            MockedTwitchClientFactory = new();
            MockedTwitchClient = new();
            MockedCredentialsManager = new();
            MockedSpinWait = new();
            MockedThreadService = new();
            MockedDateTimeService = new();
            ApplicationCredentials = new();
            ConnectionCredentials = new(TwitchUsername, TwitchOAuth, disableUsernameCheck: true);
            LoginCredentials = new(ConnectionCredentials, true, PrimaryChannelName);
            MockedWriter = new();

            Sut = new TwitchClientManager(
                MockedApplicationContextFactory.Object,
                MockedTwitchClientFactory.Object,
                MockedCredentialsManager.Object,
                MockedSpinWait.Object,
                MockedThreadService.Object,
                MockedDateTimeService.Object,
                MockedWriter.Object);

            MockedTwitchClientFactory
                .Setup(x => x.CreateClient(It.IsAny<IClientOptions>()))
                .Returns(MockedTwitchClient.Object);

            var credentials = LoginCredentials;
            MockedCredentialsManager
                .Setup(x => x.TryLoginAsNewUser())
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
