using Moq;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Bots
{
    public sealed class WhenStarting : UsingBot
    {
        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, false)]
        [InlineData(true, true, true)]
        public void ShouldJoinChannelIfBothInitializeClientAndJoinChannels(
            Boolean successfullyInitializeClient,
            Boolean successfullyJoinChannels,
            Boolean expectedSucess)
        {
            MockedTwitchClientManager
                .Setup(x => x.TryInitialize(Sut, It.IsAny<IClientOptions>(), It.IsAny<Boolean>()))
                .Returns(successfullyInitializeClient);

            MockedTwitchClientManager
                .Setup(x => x.TryJoinInitialChannels(It.IsAny<IReadOnlyList<JoinedChannel>>()))
                .Returns(successfullyJoinChannels);

            Boolean result = Sut.Start();

            Assert.Equal(expectedSucess, result);
        }
    }
}
