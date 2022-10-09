using Chubberino.Infrastructure.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.TimeoutAlerts;

public sealed class WhenOnUserTimedout : UsingTimeoutAlert
{
    [Fact]
    public void ShouldSpoolCorrectMessage()
    {
        var args = new OnUserTimedoutArgs()
        {
            UserTimeout = new UserTimeout(
                channel: Guid.NewGuid().ToString(),
                username: Guid.NewGuid().ToString(),
                timeoutDuration: new Random().Next(),
                timeoutReason: Guid.NewGuid().ToString())
        };

        Sut.TwitchClient_OnUserTimedout(null, args);

        MockedTwitchClientManager
            .Verify(x => x.SpoolMessage(PrimaryChannelName, $"WideHardo FREE MY MAN {args.UserTimeout.Username.ToUpper()}", Priority.Medium),
                Times.Once());
    }
}
