using System;
using Chubberino.Infrastructure.Credentials;
using Microsoft.Extensions.Logging;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Interfaces;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Infrastructure.Client.TwitchClients;

public sealed class TwitchClientFactory : ITwitchClientFactory
{
    private Func<IClientOptions, IClient> ClientFactory { get; }

    private ClientProtocol ClientProtocol { get; }

    private ILogger<TwitchClient> Logger { get; }

    private IClientOptions CurrentClientOptions { get; set; }

    private ICredentialsManager CredentialsManager { get; }

    public TwitchClientFactory(
        Func<IClientOptions, IClient> clientFactory,
        ClientProtocol clientProtocol,
        ILogger<TwitchClient> logger,
        ICredentialsManager credentialsManager)
    {
        ClientFactory = clientFactory;
        ClientProtocol = clientProtocol;
        Logger = logger;
        CredentialsManager = credentialsManager;
    }

    public ITwitchClient CreateClient(IClientOptions options)
    {
        var client = new TwitchClient(
            ClientFactory(options),
            ClientProtocol,
            Logger);

        if (TryInitializeTwitchClient(options))
        {
            return client;
        }

        // TODO replace
        throw new ApplicationException("Could not initialize client.");
    }

    public Boolean TryInitializeTwitchClient(IClientOptions clientOptions = null)
    {
        // We need to get all the channel that the old client was connected to,
        // so we can rejoin those channels on the new client.
        //var previouslyJoinedChannels = Client == null
        //    ? Array.Empty<JoinedChannel>()
        //    : Client.JoinedChannels.ToArray();

        if (clientOptions is not null)
        {
            CurrentClientOptions = clientOptions;
        }

        // Try to get new credentials if not provided
        return CredentialsManager.LoginCredentials.IsSome || CredentialsManager.TryUpdateLoginCredentials(null, out _);
    }
}
