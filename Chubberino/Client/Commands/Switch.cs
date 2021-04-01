using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Client.Commands
{
    public sealed class Switch : Command
    {
        public Switch(ITwitchClientManager client, IConsole console) : base(client, console)
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
}
