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
        protected Mock<IApplicationContext> MockedContext { get; }

        protected Mock<ITwitchClientManager> MockedTwitchClientManager { get; }

        protected Mock<IExtendedClient> MockedTwitchClient { get; }

        protected Mock<IRepeater> MockedRepeater { get; }

        protected Mock<IConsole> MockedConsole { get; }

        protected Mock<ICommandRepository> MockedCommandRepository { get; }

        protected Mock<IBot> MockedBot { get; }

        public UsingCommand()
        {
            MockedBot = new Mock<IBot>().SetupAllProperties();

            var moderatorOptions =  new ClientOptions();
            var regularOptions = new ClientOptions();

            MockedTwitchClientManager.Object.PrimaryChannelName = Guid.NewGuid().ToString();

            MockedConsole = new Mock<IConsole>();

            MockedContext = new Mock<IApplicationContext>();

            MockedTwitchClient = new Mock<IExtendedClient>();

            MockedTwitchClientManager = new Mock<ITwitchClientManager>();

            MockedTwitchClientManager.Setup(x => x.Client).Returns(MockedTwitchClient.Object);

            MockedRepeater = new Mock<IRepeater>().SetupAllProperties();

            MockedCommandRepository = new Mock<ICommandRepository>().SetupAllProperties();
        }

    }
}
