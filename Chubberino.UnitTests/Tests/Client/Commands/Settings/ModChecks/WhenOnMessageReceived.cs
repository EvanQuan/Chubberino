using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Client.Models.Builders;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.ModChecks;

public sealed class WhenOnMessageReceived : UsingModCheck
{
    private ChatMessage ChatMessage { get; }
    private String ExpectedMessage { get; }
    private String ExpectedDisplayName { get; }

    public WhenOnMessageReceived()
    {
        ExpectedMessage = Guid.NewGuid().ToString();
        ExpectedDisplayName = Guid.NewGuid().ToString();
        ChatMessage = ChatMessageBuilder
            .Create()
            .WithTwitchLibMessage(TwitchLibMessageBuilder
                .Create()
                .WithDisplayName(ExpectedDisplayName))
            .WithMessage(ExpectedMessage)
            .Build();
    }

    [Fact]
    public void ShouldDisableAllSettings()
    {
        MockedStopSettingStrategy
            .Setup(x => x.ShouldStop(It.IsAny<ChatMessage>()))
            .Returns(true);

        Sut.Client_OnMessageReceived(null, new OnMessageReceivedArgs() { ChatMessage = ChatMessage });

        MockedStopSettingStrategy.Verify(x => x.ShouldStop(ChatMessage), Times.Once());

        MockedCommandRepository.Verify(x => x.DisableAllSettings(), Times.Once());

        MockedWriter.Verify(x => x.WriteLine("! ! ! DISABLED ALL SETTINGS ! ! !"), Times.Once());
        MockedWriter.Verify(x => x.WriteLine($"Moderator {ExpectedDisplayName} said: \"{ExpectedMessage}\""), Times.Once());
    }

    [Fact]
    public void ShouldDoNothing()
    {
        MockedStopSettingStrategy
            .Setup(x => x.ShouldStop(It.IsAny<ChatMessage>()))
            .Returns(false);

        Sut.Client_OnMessageReceived(null, new OnMessageReceivedArgs() { ChatMessage = ChatMessage });

        MockedStopSettingStrategy.Verify(x => x.ShouldStop(ChatMessage), Times.Once());

        MockedCommandRepository.Verify(x => x.DisableAllSettings(), Times.Never());

        MockedWriter.Verify(x => x.WriteLine(It.IsAny<String>()), Times.Never());
    }
}
