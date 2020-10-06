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

            MockedExtendedClientFactory.Verify(x => x.GetClient(RegularClientOptions), Times.Once());
            MockedClient.Verify(x => x.Initialize(Credentials, Sut.ChannelName, '!', '!', true), Times.Once());
            MockedClient.Verify(x => x.JoinChannel(Sut.ChannelName, false), Times.Once());
            MockedCommandRepository.Verify(x => x.RefreshAll(MockedClient.Object), Times.Once());
        }
    }
}
