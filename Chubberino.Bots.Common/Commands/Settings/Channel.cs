using System.IO;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings;

namespace Chubberino.Bots.Common.Commands.Settings;

public sealed class Channel : Setting
{
    public override String Status => $"\n\tprimary: {TwitchClientManager.PrimaryChannelName}"
        + $"\n\tOthers:"
        + "\n\t\t" + String.Join("\n\t\t", TwitchClientManager.Client.JoinedChannels.Where(x => x.Channel != TwitchClientManager.PrimaryChannelName).Select(x => x.Channel));

    public Channel(ITwitchClientManager client, TextWriter writer) : base(client, writer)
    {
    }

    public override void Execute(IEnumerable<String> arguments)
    {
        // Cannot disable
    }

    public override Boolean Set(String property, IEnumerable<String> arguments)
    {
        var primaryChannelName = arguments.FirstOrDefault();

        if (primaryChannelName is null) { return false; }

        switch (property?.ToLower())
        {
            case "p":
            case "primary":
                var channel = TwitchClientManager.Client.JoinedChannels
                    .FirstOrDefault(x => x.Channel.Equals(primaryChannelName, StringComparison.OrdinalIgnoreCase))
                    ?.Channel ?? null;

                if (channel is not null)
                {
                    TwitchClientManager.PrimaryChannelName = channel;
                    return true;
                }
                return false;
        }
        return false;
    }
}
