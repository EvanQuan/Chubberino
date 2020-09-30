using Chubberino.Client.Commands;

namespace Chubberino.UnitTests.Tests.Client.Commands.Joins
{
    public abstract class UsingJoin : UsingCommand
    {
        protected Join Sut { get; private set; }

        public UsingJoin()
        {
            Sut = new Join(MockedTwitchClient.Object, MockedConsole.Object);
        }
    }
}
