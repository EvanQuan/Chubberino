using System;
using Monad;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Infrastructure.Client.TwitchClients;

public sealed class OnTwitchClientRefreshedArgs : EventArgs
{
    public OnTwitchClientRefreshedArgs(OptionResult<ITwitchClient> oldClient, ITwitchClient newClient)
    {
        OldClient = oldClient;
        NewClient = newClient;
    }

    public OptionResult<ITwitchClient> OldClient { get; }

    public ITwitchClient NewClient { get; }
}
