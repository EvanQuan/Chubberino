using Chubberino.Bots.Common.Commands.Settings;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.TimeoutAlerts;

public abstract class UsingTimeoutAlert : UsingCommand
{
    protected TimeoutAlert Sut { get; }

    public UsingTimeoutAlert()
    {
        Sut = new TimeoutAlert(MockedTwitchClientManager.Object, MockedWriter.Object);
    }
}
