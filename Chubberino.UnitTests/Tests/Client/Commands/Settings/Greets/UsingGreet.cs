using Chubberino.Client.Commands.Settings;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Greets
{
    public abstract class UsingGreet : UsingCommand
    {
        protected Greet Sut { get; }

        public UsingGreet()
        {
            Sut = new Greet(MockedTwitchClient.Object, MockedConsole.Object);
        }
    }
}
