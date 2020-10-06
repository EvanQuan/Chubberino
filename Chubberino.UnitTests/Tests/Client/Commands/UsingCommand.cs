using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Moq;
using System;
using System.IO;
using TwitchLib.Communication.Models;

namespace Chubberino.UnitTests.Tests.Client.Commands
{
    public abstract class UsingCommand
    {
        protected Mock<IExtendedClient> MockedTwitchClient { get; }

        protected Mock<IRepeater> MockedRepeater { get; }

        protected Mock<TextWriter> MockedConsole { get; }

        protected Mock<ICommandRepository> MockedCommandRepository { get; }

        protected BotInfo BotInfo { get; }

        public UsingCommand()
        {
            BotInfo = new BotInfo(new ClientOptions(), new ClientOptions())
            {
                ChannelName = Guid.NewGuid().ToString()
            };

            MockedConsole = new Mock<TextWriter>().SetupAllProperties();

            MockedTwitchClient = new Mock<IExtendedClient>().SetupAllProperties();

            MockedRepeater = new Mock<IRepeater>().SetupAllProperties();

            MockedCommandRepository = new Mock<ICommandRepository>().SetupAllProperties();
        }

    }
}
