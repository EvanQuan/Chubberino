using Chubberino.Bots.Channel.Commands;

namespace Chubberino.UnitTests.Tests.Client.Commands.Joins;

public abstract class UsingJoin : UsingCommand
{
    protected Join Sut { get; private set; }

    public UsingJoin()
    {
        Sut = new Join(MockedContextFactory.Object, MockedTwitchClientManager.Object, MockedWriter.Object);
    }
}
