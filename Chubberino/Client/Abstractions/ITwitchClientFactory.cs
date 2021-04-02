using TwitchLib.Client.Interfaces;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client.Abstractions
{
    public interface ITwitchClientFactory
    {
        ITwitchClient GetClient(IClientOptions options);
    }
}
