using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands;
using Moq;
using System.IO;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    public abstract class UsingCommandRepository
    {
        /// <summary>
        /// System under test.
        /// </summary>
        protected CommandRepository Sut { get; }

        protected Mock<IExtendedClient> MockedClient { get; }

        protected Mock<TextWriter> MockedConsole { get; }

        protected Mock<IBot> MockedBot { get; }

        public UsingCommandRepository()
        {
            MockedClient = new Mock<IExtendedClient>();

            MockedConsole = new Mock<TextWriter>();

            MockedBot = new Mock<IBot>();

            Sut = new CommandRepository(MockedClient.Object, MockedConsole.Object, MockedBot.Object);
        }
    }
}
