using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Communication.Models;

namespace Chubberino.UnitTests.Tests.Client.Bots
{
    public abstract class UsingBot
    {
        protected Bot Sut { get; }

        protected Mock<TextWriter> MockedConsole { get; }

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

        public UsingBot()
        {
            MockedConsole = new Mock<TextWriter>().SetupAllProperties();

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

            MockedExtendedClientFactory
                .Setup(x => x.GetClient(It.IsAny<IClientOptions>()))
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
                    Sut.ChannelName = channel;
                });

            Sut = new Bot(
                MockedConsole.Object,
                MockedCommandRepository.Object,
                Credentials,
                ModeratorClientOptions,
                RegularClientOptions,
                MockedExtendedClientFactory.Object,
                ChannelName);

            MockedExtendedClientFactory.Invocations.Clear();
            MockedCommandRepository.Invocations.Clear();
            MockedClient.Invocations.Clear();
        }
    }
}
