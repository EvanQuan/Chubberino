using Chubberino.Client.Commands;

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
            Sut = new DisableAll(MockedTwitchClientManager.Object, MockedCommandRepository.Object, MockedConsole.Object);
        }
    }
}
