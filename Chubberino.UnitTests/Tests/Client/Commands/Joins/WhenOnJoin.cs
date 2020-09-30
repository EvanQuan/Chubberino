using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Events;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Joins
{
    public sealed class WhenOnJoin : UsingJoin
    {
        public WhenOnJoin()
        {
            
        }

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
    }
}
