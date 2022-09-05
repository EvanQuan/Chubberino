using Chubberino.Infrastructure.Client.TwitchClients;
using Moq;
using TwitchLib.Client.Interfaces;

namespace Chubberino.UnitTestQualityTools.Extensions;

public static class MockedITwitchClientManagerExtensions
{
    public static Mock<ITwitchClient> SetupClient(this Mock<ITwitchClientManager> source)
    {
        Mock<ITwitchClient> client = new();

        client.SetupAllProperties();

        source.Setup(x => x.Client).Returns(client.Object);

        return client;
    }
}
