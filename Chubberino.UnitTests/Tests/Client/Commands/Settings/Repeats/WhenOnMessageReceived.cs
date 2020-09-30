using Moq;
using System;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models.Builders;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Repeats
{
    public sealed class WhenOnMessageReceived : UsingRepeat
    {
        private String ExpectedMessage { get; }
        private String ExpectedBotUsername { get; }

        public WhenOnMessageReceived()
        {
            ExpectedMessage = Guid.NewGuid().ToString();
            ExpectedBotUsername = Guid.NewGuid().ToString();

            Sut.WaitingForRepeatMessage = true;
        }

        [Fact]
        public void ShouldRecordRepeatMessage()
        {
            var chatMessage = ChatMessageBuilder
                .Create()
                .WithTwitchLibMessage(TwitchLibMessageBuilder
                    .Create()
                    .WithBotUserName(ExpectedBotUsername)
                    .WithUsername(ExpectedBotUsername)
                    .Build())
                .WithMessage(ExpectedMessage)
                .Build();

            Sut.TwitchClient_OnMessageReceived(null, new OnMessageReceivedArgs { ChatMessage = chatMessage });

            MockedConsole.Verify(x => x.WriteLine($"Received repeat message: \"{ExpectedMessage}\""), Times.Once());
            Assert.False(Sut.WaitingForRepeatMessage);
            Assert.Equal(ExpectedMessage, Sut.RepeatMessage);
        }

        [Fact]
        public void ShouldDoNothing()
        {
            var chatMessage = ChatMessageBuilder
                .Create()
                .WithTwitchLibMessage(TwitchLibMessageBuilder
                    .Create()
                    .WithBotUserName(ExpectedBotUsername)
                    .WithUsername(String.Empty)
                    .Build())
                .WithMessage(ExpectedMessage)
                .Build();

            Sut.TwitchClient_OnMessageReceived(null, new OnMessageReceivedArgs { ChatMessage = chatMessage });

            MockedConsole.Verify(x => x.WriteLine(It.IsAny<String>()), Times.Never());
            Assert.True(Sut.WaitingForRepeatMessage);
            Assert.Equal(default, Sut.RepeatMessage);
        }
    }
}
