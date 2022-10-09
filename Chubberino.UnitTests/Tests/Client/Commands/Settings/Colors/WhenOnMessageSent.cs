using Chubberino.Infrastructure.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models.Builders;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Colors;

public sealed class WhenOnMessageSent : UsingColor
{
    public WhenOnMessageSent()
    {
        Sut.AddColorSelector(MockedSelector1.Object);
    }

    [Fact]
    public void ShouldIgnoreCommands()
    {
        var args = new OnMessageSentArgs()
        {
            SentMessage = SentMessageBuilder
                .Create()
                .WithMessage('.' + Message)
                .Build()
        };

        Sut.TwitchClient_OnMessageSent(null, args);

        MockedTwitchClientManager
            .Verify(x => x.SpoolMessage(PrimaryChannelName, It.IsAny<String>(), Priority.Medium), Times.Never());
    }

    [Fact]
    public void ShouldSetColorFromColorSelector()
    {
        var args = new OnMessageSentArgs()
        {
            SentMessage = SentMessageBuilder
                .Create()
                .WithMessage(Message)
                .Build()
        };

        Sut.TwitchClient_OnMessageSent(null, args);

        MockedTwitchClientManager
            .Verify(x => x.SpoolMessage(PrimaryChannelName, $".color {Color1}", Priority.Medium), Times.Once());
    }
}
