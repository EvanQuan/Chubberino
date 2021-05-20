using Chubberino.Client;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client
{
    public sealed class WhenSpoolingMessage : UsingTwitchClientManager
    {
        public WhenSpoolingMessage()
        {
            Sut.TryInitialize(MockedBot.Object, credentials: LoginCredentials);
        }

        [Fact]
        public void ShouldSplitMessageBeyondMessageLengthLimit()
        {
            var channelName = Guid.NewGuid().ToString();

            var message = String.Concat(Enumerable.Repeat("a ", TwitchClientManager.MaxMessageLength/2 + 1));

            var expectedFirstMessage = String.Concat(Enumerable.Repeat("a ", TwitchClientManager.MaxMessageLength/2 - 1)) + 'a';
            var expectedSecondMessage = "a ";

            Sut.SpoolMessage(channelName, message, Priority.Medium);

            MockedTwitchClient.Verify(x => x.SendMessage(channelName, expectedFirstMessage, false), Times.Once());
            MockedTwitchClient.Verify(x => x.SendMessage(channelName, expectedSecondMessage, false), Times.Once());
        }
    }
}
