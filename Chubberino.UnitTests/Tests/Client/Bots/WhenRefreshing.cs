using Moq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Bots
{
    public sealed class WhenRefreshing : UsingBot
    {
        [Fact]
        public void ShouldRefreshComponentsWithOptions()
        {
            Sut.Refresh(RegularClientOptions);

            MockedTwitchClientManager.Verify(x => x.TryInitializeClient(Sut, RegularClientOptions, false), Times.Once());
            MockedClient.Verify(x => x.Initialize(Credentials, MockedTwitchClientManager.Object.PrimaryChannelName, '!', '!', true), Times.Once());
            MockedClient.Verify(x => x.JoinChannel(MockedTwitchClientManager.Object.PrimaryChannelName, false), Times.Once());
            MockedCommandRepository.Verify(x => x.RefreshAll(), Times.Once());
        }
    }
}
