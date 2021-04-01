using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Moq;
using System;
using TwitchLib.Client.Interfaces;
using TwitchLib.Communication.Models;

namespace Chubberino.UnitTests.Tests.Client.Commands
{
    public abstract class UsingCommand
    {
        protected Mock<IApplicationContext> MockedContext { get; }

        protected Mock<ITwitchClientManager> MockedTwitchClientManager { get; }

        protected Mock<ITwitchClient> MockedTwitchClient { get; }

        protected Mock<IRepeater> MockedRepeater { get; }

        protected Mock<IConsole> MockedConsole { get; }

        protected Mock<ICommandRepository> MockedCommandRepository { get; }

        protected Mock<IBot> MockedBot { get; }

        protected String PrimaryChannelName { get; }

        public UsingCommand()
        {
            PrimaryChannelName = Guid.NewGuid().ToString();

            MockedBot = new Mock<IBot>().SetupAllProperties();

            var moderatorOptions =  new ClientOptions();
            var regularOptions = new ClientOptions();

            MockedConsole = new Mock<IConsole>();

            MockedContext = new Mock<IApplicationContext>();

            MockedTwitchClient = new Mock<ITwitchClient>();

            MockedTwitchClientManager = new Mock<ITwitchClientManager>();

            MockedTwitchClientManager.Setup(x => x.PrimaryChannelName).Returns(PrimaryChannelName);

            MockedTwitchClientManager.Setup(x => x.Client).Returns(MockedTwitchClient.Object);

            MockedRepeater = new Mock<IRepeater>().SetupAllProperties();

            MockedCommandRepository = new Mock<ICommandRepository>().SetupAllProperties();
        }

    }
}
