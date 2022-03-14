using Chubberino.Bots.Common.Commands;
using Chubberino.Bots.Common.Commands.Settings;
using Moq;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Greets
{
    public abstract class UsingGreet : UsingCommand
    {
        protected Greet Sut { get; }

        protected Mock<IComplimentGenerator> MockedCompliments { get; }

        public UsingGreet()
        {
            MockedCompliments = new();

            Sut = new Greet(MockedTwitchClientManager.Object, MockedWriter.Object, MockedCompliments.Object);
        }
    }
}
