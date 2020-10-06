using Chubberino.Client.Abstractions;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    public sealed class ExtendedClientFactory : IExtendedClientFactory
    {
        private IBot Bot { get; }

        public ExtendedClientFactory(IBot bot)
        {
            Bot = bot;
        }

        public IExtendedClient GetClient(IClientOptions options)
        {
            return new ExtendedClient(Bot, new WebSocketClient(options));
        }
    }
}
