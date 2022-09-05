using TwitchLib.Client.Interfaces;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Infrastructure.Client.TwitchClients;

public interface ITwitchClientFactory
{
    ITwitchClient CreateClient(IClientOptions options);
}
