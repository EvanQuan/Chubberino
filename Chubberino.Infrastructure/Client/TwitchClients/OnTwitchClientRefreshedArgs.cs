using TwitchLib.Client.Interfaces;

namespace Chubberino.Infrastructure.Client.TwitchClients;

public sealed class OnTwitchClientRefreshedArgs : EventArgs
{
    public OnTwitchClientRefreshedArgs(Option<ITwitchClient> oldClient, ITwitchClient newClient)
    {
        OldClient = oldClient;
        NewClient = newClient;
    }

    public Option<ITwitchClient> OldClient { get; }

    public ITwitchClient NewClient { get; }
}
