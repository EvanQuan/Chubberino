using Moq;
using System;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models.Builders;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Colors
{
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
                .Verify(x => x.SpoolMessage(It.IsAny<String>()), Times.Never());
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
                .Verify(x => x.SpoolMessage($".color {Color1}"), Times.Once());
        }
    }
}
