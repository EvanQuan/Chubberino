using Chubberino.Client;
using Moq;
using TwitchLib.Client.Interfaces;

namespace Chubberino.UnitTests.Utility
{
    public static class MockedITwitchClientManagerExtensions
    {
        public static Mock<ITwitchClient> SetupClient(this Mock<ITwitchClientManager> source)
        {
            Mock<ITwitchClient> client = new();

            source.Setup(x => x.Client).Returns(client.Object);

            return client;
        }
    }
}
