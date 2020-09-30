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

        public UsingDisableAll()
        {
            Sut = new DisableAll(MockedTwitchClient.Object, MockedCommandRepository.Object, MockedConsole.Object);
        }
    }
}
