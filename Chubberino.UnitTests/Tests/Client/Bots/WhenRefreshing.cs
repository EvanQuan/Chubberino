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

            MockedExtendedClientFactory.Verify(x => x.GetClient(Sut, RegularClientOptions), Times.Once());
            MockedClient.Verify(x => x.Initialize(Credentials, Sut.PrimaryChannelName, '!', '!', true), Times.Once());
            MockedClient.Verify(x => x.JoinChannel(Sut.PrimaryChannelName, false), Times.Once());
            MockedCommandRepository.Verify(x => x.RefreshAll(MockedClient.Object), Times.Once());
        }
    }
}
