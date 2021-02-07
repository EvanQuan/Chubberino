using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands
{
    public sealed class Join : Command
    {
        public Join(IExtendedClient client, TextWriter console)
            : base(client, console)
        {
            TwitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;
        }

        public void TwitchClient_OnJoinedChannel(Object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine($"Joined channel {e.Channel}");
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (!arguments.Any()) { return; }

            if (!TwitchClient.IsConnected)
            {
                TwitchClient.Connect();
            }

            String channelName = arguments.FirstOrDefault();

            // If the user inputs any second argument, it will join that channel and not leave the existing channel.

            TwitchClient.EnsureJoinedToChannel(channelName);
        }

        public override string GetHelp()
        {
            return @"
Join a twitch channel.

usage: join <channel name>

    <channel name>                      Name of the twitch channel to join.
";
        }
    }
}
