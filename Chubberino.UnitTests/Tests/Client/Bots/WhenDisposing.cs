using Moq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Bots
{
    public sealed class WhenDisposing : UsingBot
    {
        [Fact]
        public void ShouldDisconnect()
        {
            MockedClient.Setup(x => x.IsConnected).Returns(true);

            Sut.Dispose();

            MockedClient.Verify(x => x.Disconnect(), Times.Once());
        }

        [Fact]
        public void ShouldNotDisonnect()
        {
            MockedClient.Setup(x => x.IsConnected).Returns(false);

            Sut.Dispose();

            MockedClient.Verify(x => x.Disconnect(), Times.Never());
        }
    }
}
