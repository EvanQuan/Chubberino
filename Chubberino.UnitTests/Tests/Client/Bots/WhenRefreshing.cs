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
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void ShouldRefreshComponentsWithOptions(Boolean regularClientOptions, Boolean askForCredentials)
        {
            IClientOptions clientOptions = regularClientOptions ? RegularClientOptions : ModeratorClientOptions;

            Sut.Refresh(clientOptions, askForCredentials);

            MockedTwitchClientManager.Verify(x => x.TryInitialize(Sut, clientOptions, askForCredentials), Times.Once());
            MockedTwitchClientManager.Verify(x => x.TryJoinInitialChannels(It.IsAny<IReadOnlyList<JoinedChannel>>()), Times.Once());
            MockedCommandRepository.Verify(x => x.RefreshAll(), Times.Once());
        }
    }
}
