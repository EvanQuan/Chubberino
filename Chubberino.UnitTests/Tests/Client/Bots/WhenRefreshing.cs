using Chubberino.Client.Credentials;
using Moq;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Bots
{
    public sealed class WhenRefreshing : UsingBot
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShouldRefreshComponentsWithOptions(Boolean regularClientOptions)
        {
            IClientOptions clientOptions = regularClientOptions ? RegularClientOptions : ModeratorClientOptions;

            Sut.Refresh(clientOptions);

            MockedTwitchClientManager.Verify(x => x.TryInitialize(Sut, clientOptions, It.IsAny<LoginCredentials>()), Times.Once());
            MockedTwitchClientManager.Verify(x => x.TryJoinInitialChannels(It.IsAny<IReadOnlyList<JoinedChannel>>()), Times.Once());
            MockedCommandRepository.Verify(x => x.RefreshAll(), Times.Once());
        }
    }
}
