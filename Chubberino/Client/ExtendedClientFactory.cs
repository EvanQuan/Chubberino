using Chubberino.Client.Abstractions;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    public sealed class ExtendedClientFactory : IExtendedClientFactory
    {
        private BotInfo BotInfo { get; }

        public ExtendedClientFactory(BotInfo botInfo)
        {
            BotInfo = botInfo;
        }

        public IExtendedClient GetClient(IClientOptions options)
        {
            return new ExtendedClient(BotInfo, new WebSocketClient(options));
        }
    }
}
