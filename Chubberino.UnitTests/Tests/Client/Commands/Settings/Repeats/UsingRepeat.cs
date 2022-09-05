using Chubberino.Bots.Common.Commands.Settings;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Repeats;

public abstract class UsingRepeat : UsingCommand
{
    protected Repeat Sut { get; }

    public UsingRepeat()
    {
        Sut = new Repeat(MockedTwitchClientManager.Object, MockedRepeater.Object, MockedWriter.Object);
    }
}
