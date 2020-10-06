using Moq;
using System;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models.Builders;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Colors
{
    public sealed class WhenOnMessageReceived : UsingColor
    {
        private String Message { get; } = Guid.NewGuid().ToString();
        private String Username { get; } = Guid.NewGuid().ToString();
        private String BotUsername { get; } = Guid.NewGuid().ToString();
        private String Color { get; } = Guid.NewGuid().ToString();

        private OnMessageReceivedArgs Args { get; set; }

        public WhenOnMessageReceived()
        {
            MockedSelector1.Setup(x => x.GetNextColor())
                .Returns(Color);

            Sut.AddColorSelector(MockedSelector1.Object);
        }

        [Fact]
        public void ShouldIgnoreNonBotUsername()
        {
            Args = new OnMessageReceivedArgs()
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

            Sut.TwitchClient_OnMessageReceived(null, Args);

            MockedTwitchClient
                .Verify(x => x.SpoolMessage(It.IsAny<String>()), Times.Never());
        }

        [Fact]
        public void ShouldSetColorFromColorSelector()
        {
            Args = new OnMessageReceivedArgs()
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

            Sut.TwitchClient_OnMessageReceived(null, Args);

            MockedTwitchClient
                .Verify(x => x.SpoolMessage($".color {Color}"), Times.Once());
        }
    }
}
