using Chubberino.Bots.Common.Commands.Settings;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Logs;

public abstract class UsingLog : UsingCommand
{
    protected Log Sut { get; private set; }

    public UsingLog()
    {
        Sut = new Log(MockedTwitchClientManager.Object, MockedWriter.Object);
    }
}
