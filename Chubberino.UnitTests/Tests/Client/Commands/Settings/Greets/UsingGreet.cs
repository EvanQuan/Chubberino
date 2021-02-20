using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings;
using Moq;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Greets
{
    public abstract class UsingGreet : UsingCommand
    {
        protected Greet Sut { get; }

        protected Mock<IComplimentGenerator> MockedCompliments { get; }

        public UsingGreet()
        {
            MockedCompliments = new Mock<IComplimentGenerator>();

            Sut = new Greet(MockedTwitchClientManager.Object, MockedConsole.Object, MockedCompliments.Object);
        }
    }
}
