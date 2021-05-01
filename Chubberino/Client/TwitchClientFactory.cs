using Microsoft.Extensions.Logging;
using System;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Interfaces;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    public sealed class TwitchClientFactory : ITwitchClientFactory
    {
        private Func<IClientOptions, IClient> ClientFactory { get; }

        private ClientProtocol ClientProtocol { get; }

        private ILogger<TwitchClient> Logger { get; }

        public TwitchClientFactory(
            Func<IClientOptions, IClient> clientFactory,
            ClientProtocol clientProtocol,
            ILogger<TwitchClient> logger)
        {
            ClientFactory = clientFactory;
            ClientProtocol = clientProtocol;
            Logger = logger;
        }

        public ITwitchClient GetClient(IClientOptions options)
        {
            return new TwitchClient(
                ClientFactory(options),
                ClientProtocol,
                Logger);
        }
    }
}
