using Chubberino.Client.Abstractions;
using Chubberino.Client.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands
{
    public sealed class Join : Command
    {
        private String JoinedChannelName { get; set; }

        private IBot Bot { get; }

        public Join(IExtendedClient client, TextWriter console, IBot bot)
            : base(client, console)
        {
            Bot = bot;
            TwitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;
        }

        public void TwitchClient_OnJoinedChannel(Object sender, OnJoinedChannelArgs e)
        {
            // For simplicity, we can only be in 1 channel at a time.
            if (e.Channel.Equals(JoinedChannelName, StringComparison.OrdinalIgnoreCase)) { return; }

            if (JoinedChannelName != null)
            {
                TwitchClient.LeaveChannel(JoinedChannelName);
                Console.WriteLine($"Left channel {JoinedChannelName}");
            }

            JoinedChannelName = e.Channel;
            Bot.ChannelName = JoinedChannelName;
            Console.WriteLine($"Joined channel {JoinedChannelName}");
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (arguments.Count() == 0) { return; }

            if (!TwitchClient.IsConnected)
            {
                TwitchClient.Connect();
            }

            String channelName = arguments.FirstOrDefault();

            TwitchClient.EnsureJoinedToChannel(channelName);
        }
    }
}
