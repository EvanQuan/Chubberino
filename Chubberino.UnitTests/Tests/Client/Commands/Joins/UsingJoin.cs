using Chubberino.Client.Commands.Settings.UserCommands;

namespace Chubberino.UnitTests.Tests.Client.Commands.Joins
{
    public abstract class UsingJoin : UsingCommand
    {
        protected Join Sut { get; private set; }

        public UsingJoin()
        {
            Sut = new Join(MockedContextFactory.Object, MockedTwitchClientManager.Object, MockedConsole.Object);
        }
    }
}
