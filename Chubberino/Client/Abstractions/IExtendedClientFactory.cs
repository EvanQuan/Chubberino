using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client.Abstractions
{
    public interface IExtendedClientFactory
    {
        IExtendedClient GetClient(IClientOptions options);
    }
}
