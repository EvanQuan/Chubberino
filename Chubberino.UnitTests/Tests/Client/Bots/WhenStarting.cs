using System;
using System.Collections.Generic;
using Chubberino.Infrastructure.Credentials;
using Moq;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Bots;

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
            .Setup(x => x.TryInitializeTwitchClient(Sut, It.IsAny<IClientOptions>(), It.IsAny<LoginCredentials>()))
            .Returns(successfullyInitializeClient ? LoginCredentials : null);

        MockedTwitchClientManager
            .Setup(x => x.TryJoinInitialChannels(It.IsAny<IReadOnlyList<JoinedChannel>>()))
            .Returns(successfullyJoinChannels);

        var result = Sut.Start();

        Assert.Equal(expectedSucess ? LoginCredentials : null, result);
    }
}
