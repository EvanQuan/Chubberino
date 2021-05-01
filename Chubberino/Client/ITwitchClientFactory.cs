using TwitchLib.Client.Interfaces;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    public interface ITwitchClientFactory
    {
        ITwitchClient GetClient(IClientOptions options);
    }
}
