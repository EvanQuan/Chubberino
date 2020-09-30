using Moq;
using System;
using TwitchLib.Client.Events;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Joins
{
    public sealed class WhenOnJoin : UsingJoin
    {
        /// <summary>
        /// Should output a message indicating a channel has been joined.
        /// With no prior joined channel, there should not be any left channel
        /// message.
        /// </summary>
        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        public void ShouldOutputJoinedChannelMessageOnly(String newChannel)
        {
            var args = new OnJoinedChannelArgs()
            {
                Channel = newChannel
            };

            Sut.TwitchClient_OnJoinedChannel(null, args);

            MockedConsole.Verify(x => x.WriteLine($"Joined channel {newChannel}"), Times.Once());
            MockedConsole.Verify(x => x.WriteLine(It.IsAny<String>()), Times.Once());
        }

        /// <summary>
        /// Should output a message indicating a channel has been joined.
        /// With a prior joined channel, there should not a left channel
        /// message with the old channel name.
        /// </summary>
        /// <param name="oldChannel"></param>
        /// <param name="newChannel"></param>
        [Theory]
        [InlineData("b", "a")]
        [InlineData("a", "b")]
        public void ShouldOutputLeftChannelAndJoinedChannelMessages(String oldChannel, String newChannel)
        {
            var args1 = new OnJoinedChannelArgs()
            {
                Channel = oldChannel
            };

            Sut.TwitchClient_OnJoinedChannel(null, args1);

            var args2 = new OnJoinedChannelArgs()
            {
                Channel = newChannel
            };

            Sut.TwitchClient_OnJoinedChannel(null, args2);

            MockedConsole.Verify(x => x.WriteLine($"Joined channel {oldChannel}"), Times.Once());
            MockedConsole.Verify(x => x.WriteLine($"Left channel {oldChannel}"), Times.Once());
            MockedConsole.Verify(x => x.WriteLine($"Joined channel {newChannel}"), Times.Once());
            MockedConsole.Verify(x => x.WriteLine(It.IsAny<String>()), Times.Exactly(3));
        }

    }
}
