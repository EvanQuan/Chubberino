using Moq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Bots
{
    public sealed class WhenDisposing : UsingBot
    {
        [Fact]
        public void ShouldDisconnect()
        {
            Sut.Start();
            MockedClient.Setup(x => x.IsConnected).Returns(true);

            Sut.Dispose();

            MockedClient.Verify(x => x.Disconnect(), Times.Once());
        }

        [Fact]
        public void ShouldNotDisconnect()
        {

            Sut.Start();
            MockedClient.Setup(x => x.IsConnected).Returns(false);

            Sut.Dispose();

            MockedClient.Verify(x => x.Disconnect(), Times.Never());
        }
    }
}
