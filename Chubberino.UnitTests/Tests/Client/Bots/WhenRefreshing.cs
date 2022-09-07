using System;
using System.Collections.Generic;
using Moq;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Bots;

public sealed class WhenRefreshing : UsingBot
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ShouldRefreshComponentsWithOptions(Boolean regularClientOptions)
    {
        IClientOptions clientOptions = regularClientOptions ? RegularClientOptions : ModeratorClientOptions;

        Sut.Refresh(clientOptions);

        MockedTwitchClientManager
            .Verify(x => x
                .TryInitializeTwitchClient(
                    Sut,
                    clientOptions,
                    LoginCredentials),
                Times.Once);
        MockedTwitchClientManager
            .Verify(x => x.TryJoinInitialChannels(It.IsAny<IReadOnlyList<JoinedChannel>>()),
            Times.Once);
    }
}
