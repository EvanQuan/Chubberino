using Moq;
using System;
using TwitchLib.Client.Models;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Bots
{
    public sealed class WhenStarting : UsingBot
    {
        [Fact]
        public void ShouldJoinChannel()
        {
            Boolean result = Sut.Start();

            Assert.True(result);
            MockedConsole.Verify(x => x.WriteLine("Connecting to " + BotInfo.ChannelName), Times.Once());
            Assert.True(MockedClient.Object.IsConnected);
        }

        [Fact]
        public void ShouldFailToJoinChannelOnFailureToConnect()
        {
            // Client fails to connect.
            MockedClient
                .Setup(x => x.Connect())
                .Callback(() => MockedClient
                    .Setup(x => x.IsConnected)
                    .Returns(false));

            Boolean result = Sut.Start();

            Assert.False(result);
            MockedConsole.Verify(x => x.WriteLine("Connecting to " + BotInfo.ChannelName), Times.Once());
        }

        [Fact]
        public void FailToJoinOnFailureToUpdateChannelName()
        {
            String initialChannelName = BotInfo.ChannelName;

            // BotInfo fails to update channel name.
            MockedClient
                .Setup(x => x.JoinChannel(It.IsAny<String>(), It.IsAny<Boolean>()))
                .Callback((String channel, Boolean overideCheck) =>
                {
                    JoinedChannels.Add(new JoinedChannel(channel));
                    BotInfo.ChannelName = null;
                });

            Boolean result = Sut.Start();

            Assert.False(result);
            MockedConsole.Verify(x => x.WriteLine("Connecting to " + initialChannelName), Times.Once());
        }
    }
}
