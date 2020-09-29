using Chubberino.Client.Commands.Settings;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.AutoChats
{
    public abstract class UsingAutoChat : UsingCommand
    {
        protected AutoChat Sut { get; }

        public UsingAutoChat()
        {
            Sut = new AutoChat(MockedTwitchClient.Object, MockedConsole.Object);
        }
    }
}
