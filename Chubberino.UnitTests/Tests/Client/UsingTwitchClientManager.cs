using Chubberino.Client;
using Chubberino.Client.Credentials;
using Chubberino.Client.Services;
using Chubberino.Client.Threading;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Moq;
using TwitchLib.Client.Interfaces;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.UnitTests.Tests.Client
{
    public abstract class UsingTwitchClientManager
    {
        protected TwitchClientManager Sut { get; }

        protected Mock<IBot> MockedBot { get; }

        protected Mock<IApplicationContext> MockedApplicationContext { get; }

        protected Mock<ITwitchClientFactory> MockedTwitchClientFactory { get; }

        protected Mock<ITwitchClient> MockedTwitchClient { get; }

        protected Mock<ICredentialsManager> MockedCredentialsManager { get; }

        protected Mock<ISpinWait> MockedSpinWait { get; }

        protected Mock<IDateTimeService> MockedDateTimeService { get; }

        protected ApplicationCredentials ApplicationCredentials { get; }

        protected Credentials Credentials { get; }

        protected Mock<IConsole> MockedConsole { get; }

        protected UsingTwitchClientManager()
        {
            MockedBot = new();
            MockedApplicationContext = new();
            MockedTwitchClientFactory = new();
            MockedTwitchClient = new();
            MockedCredentialsManager = new();
            MockedSpinWait = new();
            MockedDateTimeService = new();
            ApplicationCredentials = new();
            Credentials = new(null, true);
            MockedConsole = new();

            Sut = new TwitchClientManager(
                MockedApplicationContext.Object,
                MockedTwitchClientFactory.Object,
                MockedCredentialsManager.Object,
                MockedSpinWait.Object,
                MockedDateTimeService.Object,
                ApplicationCredentials,
                MockedConsole.Object);

            MockedTwitchClientFactory
                .Setup(x => x.GetClient(It.IsAny<IClientOptions>()))
                .Returns(MockedTwitchClient.Object);

            var credentials = Credentials;
            MockedCredentialsManager
                .Setup(x => x.TryGetCredentials(out credentials))
                .Returns(true);
        }
    }
}
