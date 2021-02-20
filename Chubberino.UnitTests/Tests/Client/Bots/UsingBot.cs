using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.UnitTests.Utility;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Communication.Models;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Bots
{
    public abstract class UsingBot
    {
        protected Bot Sut { get; }

        protected Mock<IApplicationContext> MockedContext { get; }

        protected Mock<IConsole> MockedConsole { get; }

        protected ConnectionCredentials Credentials { get; }

        protected String Username { get; }

        protected String TwitchOAuth { get; }

        protected String ChannelName { get; }

        protected Mock<ICommandRepository> MockedCommandRepository { get; }

        protected Mock<IExtendedClientFactory> MockedExtendedClientFactory { get; }

        protected ClientOptions RegularClientOptions { get; }

        protected ClientOptions ModeratorClientOptions { get; }

        protected Mock<IExtendedClient> MockedClient { get; }

        protected List<JoinedChannel> JoinedChannels { get; }

        protected Mock<ISpinWait> MockedSpinWait { get; }

        protected IList<StartupChannel> StartupChannels { get; }

        protected IList<Player> Players { get; }

        public UsingBot()
        {
            StartupChannels = new List<StartupChannel>
            {
                new StartupChannel()
                {
                    ID = 1,
                    UserID = "1",
                    DisplayName = "a"
                }
            };

            Players = new List<Player>
            {
                new Player()
                {
                    ID = 1,
                    TwitchUserID = "1",
                    Name = "a"
                }
            };

            MockedContext = new Mock<IApplicationContext>();

            MockedContext.SetupGet(x => x.StartupChannels).Returns(() => StartupChannels.ToDbSet());
            MockedContext.SetupGet(x => x.Players).Returns(() => Players.ToDbSet());

            MockedConsole = new Mock<IConsole>();

            JoinedChannels = new List<JoinedChannel>();

            Username = Guid.NewGuid().ToString();
            TwitchOAuth = Guid.NewGuid().ToString();
            ChannelName = Guid.NewGuid().ToString();

            Credentials = new ConnectionCredentials(Username, TwitchOAuth, disableUsernameCheck: true);

            MockedCommandRepository = new Mock<ICommandRepository>().SetupAllProperties();

            MockedExtendedClientFactory = new Mock<IExtendedClientFactory>().SetupAllProperties();

            MockedClient = new Mock<IExtendedClient>();

            RegularClientOptions = new ClientOptions();

            ModeratorClientOptions = new ClientOptions();

            MockedSpinWait = new Mock<ISpinWait>();

            MockedSpinWait
                .Setup(x => x.SpinUntil(It.IsAny<Func<Boolean>>(), It.IsAny<TimeSpan>()))
                .Returns((Func<Boolean> func, TimeSpan timeout) => func());

            MockedExtendedClientFactory
                .Setup(x => x.GetClient(It.IsAny<IBot>(), It.IsAny<IClientOptions>()))
                .Returns(MockedClient.Object);

            MockedClient
                .Setup(x => x.Connect())
                .Callback(() =>
                {
                    MockedClient
                        .Setup(x => x.IsConnected)
                        .Returns(true);
                });

            MockedClient
                .Setup(x => x.JoinedChannels)
                .Returns(JoinedChannels);

            MockedClient
                .Setup(x => x.JoinChannel(It.IsAny<String>(), It.IsAny<Boolean>()))
                .Callback((String channel, Boolean overrideCheck) =>
                {
                    JoinedChannels.Add(new JoinedChannel(channel));
                    if (Sut.PrimaryChannelName == null)
                    {
                        Sut.PrimaryChannelName = channel;
                    }
                });

            MockedClient
                .Setup(x => x.EnsureJoinedToChannel(It.IsAny<String>()))
                .Callback((String channel) =>
                {
                    MockedClient.Object.JoinChannel(channel);
                    MockedClient.Setup(x => x.IsConnected).Returns(true);
                })
                .Returns(true);

            Sut = new Bot(
                MockedContext.Object,
                MockedConsole.Object,
                MockedCommandRepository.Object,
                Credentials,
                ModeratorClientOptions,
                RegularClientOptions,
                MockedExtendedClientFactory.Object,
                MockedSpinWait.Object,
                ChannelName);

            MockedExtendedClientFactory.Invocations.Clear();
            MockedCommandRepository.Invocations.Clear();
            MockedClient.Invocations.Clear();
        }
    }
}
