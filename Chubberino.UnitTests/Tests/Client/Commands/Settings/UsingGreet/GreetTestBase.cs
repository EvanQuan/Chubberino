using Chubberino.Client.Commands.Settings;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.UsingGreet
{
    public abstract class GreetTestBase : CommandTestBase
    {
        protected Greet Sut { get; }

        public GreetTestBase()
        {
            Sut = new Greet(MockedTwitchClient.Object);
        }
    }
}
