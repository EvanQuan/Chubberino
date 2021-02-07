using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chubberino.Client.Commands
{
    public sealed class Switch : Command
    {
        public Switch(IExtendedClient client, TextWriter console) : base(client, console)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (!arguments.Any()) { return; }

            if (!TwitchClient.IsConnected)
            {
                TwitchClient.Connect();
            }

            String channelName = arguments.First();

            var joinedChannels = TwitchClient.JoinedChannels;

            foreach (var channel in joinedChannels)
            {
                TwitchClient.LeaveChannel(channel);
            }

            TwitchClient.EnsureJoinedToChannel(channelName);
        }
    }
}
