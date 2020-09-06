using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands;
using Moq;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    public abstract class UsingCommandRepository
    {
        /// <summary>
        /// System under test.
        /// </summary>
        protected CommandRepository Sut { get; }

        protected Mock<IExtendedClient> MockedClient { get; }

        public UsingCommandRepository()
        {
            MockedClient = new Mock<IExtendedClient>();

            Sut = new CommandRepository(MockedClient.Object);
        }
    }
}
