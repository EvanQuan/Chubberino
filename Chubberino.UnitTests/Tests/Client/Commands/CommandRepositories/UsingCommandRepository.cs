using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands;
using Moq;
using System.IO;
using TwitchLib.Client.Interfaces;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    public abstract class UsingCommandRepository
    {
        /// <summary>
        /// System under test.
        /// </summary>
        protected CommandRepository Sut { get; }

        protected Mock<ITwitchClient> MockedClient { get; }

        protected Mock<IConsole> MockedConsole { get; }

        protected Mock<IBot> MockedBot { get; }

        public UsingCommandRepository()
        {
            MockedClient = new Mock<ITwitchClient>();

            MockedConsole = new Mock<IConsole>();

            MockedBot = new Mock<IBot>();

            Sut = new CommandRepository(MockedConsole.Object);
        }
    }
}
