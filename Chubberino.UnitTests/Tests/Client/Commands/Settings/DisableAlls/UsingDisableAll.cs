using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands;
using Moq;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.DisableAlls
{
    public abstract class UsingDisableAll : UsingCommand
    {
        /// <summary>
        /// System under test.
        /// </summary>
        protected DisableAll Sut { get; }

        protected Mock<ICommandRepository> MockedCommandRepository { get; }

        public UsingDisableAll()
        {
            MockedCommandRepository = new Mock<ICommandRepository>();

            Sut = new DisableAll(MockedTwitchClient.Object, MockedCommandRepository.Object, MockedConsole.Object);
        }
    }
}
