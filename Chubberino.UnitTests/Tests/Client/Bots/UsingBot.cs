using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Moq;
using System;
using System.IO;
using TwitchLib.Client.Models;
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

        protected Mock<ICommandRepository> MockedCommandRepository { get; }

        protected Mock<IExtendedClientFactory> MockedExtendedClientFactory { get; }

        public UsingBot()
        {
            MockedConsole = new Mock<TextWriter>().SetupAllProperties();

            Username = Guid.NewGuid().ToString();
            TwitchOAuth = Guid.NewGuid().ToString();

            MockedCommandRepository = new Mock<ICommandRepository>().SetupAllProperties();

            MockedExtendedClientFactory = new Mock<IExtendedClientFactory>().SetupAllProperties();

            Sut = new Bot(
                MockedConsole.Object,
                MockedCommandRepository.Object,
                new ConnectionCredentials(Username, TwitchOAuth, disableUsernameCheck: true),
                new BotInfo(new ClientOptions(), new ClientOptions()),
                MockedExtendedClientFactory.Object);
        }
    }
}
