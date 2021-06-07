using Chubberino.Client;
using Chubberino.Client.Commands;
using Chubberino.UnitTests.Utility;
using Moq;
using TwitchLib.Client.Interfaces;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    public abstract class UsingCommandRepository
    {
        /// <summary>
        /// System under test.
        /// </summary>
        protected CommandRepository Sut { get; }

        protected Mock<ITwitchClientManager> MockedClientManager { get; }

        protected Mock<ITwitchClient> MockedClient { get; }

        protected Mock<IConsole> MockedConsole { get; }

        protected Mock<IBot> MockedBot { get; }

        public UsingCommandRepository()
        {
            MockedClientManager = new();
            MockedClient = MockedClientManager.SetupClient();

            MockedConsole = new Mock<IConsole>();

            MockedBot = new Mock<IBot>();

            Sut = new CommandRepository(MockedConsole.Object, MockedClientManager.Object);
        }
    }
}
