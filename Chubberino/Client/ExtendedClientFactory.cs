using Chubberino.Client.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    public sealed class ExtendedClientFactory : IExtendedClientFactory
    {
        private Func<IClientOptions, IClient> ClientFactory { get; }

        private ClientProtocol ClientProtocol { get; }

        private TextWriter Console { get; }

        private ISpinWait SpinWait { get; }

        private ILogger<TwitchClient> Logger { get; }

        public ExtendedClientFactory(
            Func<IClientOptions, IClient> clientFactory,
            ClientProtocol clientProtocol,
            TextWriter console,
            ISpinWait spinWait,
            ILogger<TwitchClient> logger)
        {
            ClientFactory = clientFactory;
            ClientProtocol = clientProtocol;
            Console = console;
            SpinWait = spinWait;
            Logger = logger;
        }

        public IExtendedClient GetClient(IBot bot, IClientOptions options)
        {
            return new ExtendedClient(
                bot,
                ClientFactory(options),
                ClientProtocol,
                Console,
                SpinWait,
                Logger);
        }
    }
}
