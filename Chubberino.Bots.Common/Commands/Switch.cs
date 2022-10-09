using System.IO;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;

namespace Chubberino.Bots.Common.Commands;

public sealed class Switch : Command
{
    public Switch(ITwitchClientManager client, TextWriter writer) : base(client, writer)
    {
    }

    public override void Execute(IEnumerable<String> arguments)
    {
        if (!arguments.Any()) { return; }

        if (!TwitchClientManager.Client.IsConnected)
        {
            TwitchClientManager.Client.Connect();
        }

        String channelName = arguments.First();

        var joinedChannels = TwitchClientManager.Client.JoinedChannels;

        foreach (var channel in joinedChannels)
        {
            TwitchClientManager.Client.LeaveChannel(channel);
        }

        TwitchClientManager.EnsureJoinedToChannel(channelName);
    }
}
