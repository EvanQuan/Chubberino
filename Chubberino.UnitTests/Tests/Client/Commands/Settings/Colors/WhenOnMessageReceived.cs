using Moq;
using System;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models.Builders;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Colors
{
    public sealed class WhenOnMessageReceived : UsingColor
    {
        public WhenOnMessageReceived()
        {
            Sut.AddColorSelector(MockedSelector1.Object);
        }

        [Fact]
        public void ShouldIgnoreNonBotUsername()
        {
            var args = new OnMessageReceivedArgs()
            {
                ChatMessage = ChatMessageBuilder
                    .Create()
                    .WithTwitchLibMessage(TwitchLibMessageBuilder
                        .Create()
                        .WithUsername(Username)
                        .WithBotUserName(BotUsername))
                    .WithMessage(Message)
                    .Build()
            };

            Sut.TwitchClient_OnMessageReceived(null, args);

            MockedTwitchClient
                .Verify(x => x.SpoolMessage(It.IsAny<String>()), Times.Never());
        }

        [Fact]
        public void ShouldSetColorFromColorSelector()
        {
            var args = new OnMessageReceivedArgs()
            {
                ChatMessage = ChatMessageBuilder
                    .Create()
                    .WithTwitchLibMessage(TwitchLibMessageBuilder
                        .Create()
                        .WithUsername(BotUsername)
                        .WithBotUserName(BotUsername))
                    .WithMessage(Message)
                    .Build()
            };

            Sut.TwitchClient_OnMessageReceived(null, args);

            MockedTwitchClient
                .Verify(x => x.SpoolMessage($".color {Color1}"), Times.Once());
        }
    }
}
