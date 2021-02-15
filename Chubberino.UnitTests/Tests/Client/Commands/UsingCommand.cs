using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Moq;
using System;
using System.IO;
using TwitchLib.Communication.Models;

namespace Chubberino.UnitTests.Tests.Client.Commands
{
    public abstract class UsingCommand
    {
        protected Mock<ApplicationContext> MockedContext { get; }

        protected Mock<IExtendedClient> MockedTwitchClient { get; }

        protected Mock<IRepeater> MockedRepeater { get; }

        protected Mock<TextWriter> MockedConsole { get; }

        protected Mock<ICommandRepository> MockedCommandRepository { get; }

        protected Mock<IBot> MockedBot { get; }

        public UsingCommand()
        {
            MockedBot = new Mock<IBot>().SetupAllProperties();

            var moderatorOptions =  new ClientOptions();
            var regularOptions = new ClientOptions();

            MockedBot
                .Setup(x => x.ModeratorClientOptions)
                .Returns(moderatorOptions);

            MockedBot
                .Setup(x => x.RegularClientOptions)
                .Returns(regularOptions);

            MockedBot.Object.PrimaryChannelName = Guid.NewGuid().ToString();

            MockedConsole = new Mock<TextWriter>().SetupAllProperties();

            MockedContext = new Mock<ApplicationContext>();

            MockedTwitchClient = new Mock<IExtendedClient>().SetupAllProperties();

            MockedRepeater = new Mock<IRepeater>().SetupAllProperties();

            MockedCommandRepository = new Mock<ICommandRepository>().SetupAllProperties();
        }

    }
}
