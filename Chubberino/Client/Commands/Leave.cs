using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands
{
    public sealed class Leave : Command
    {
        private IBot Bot { get; }

        public Leave(IExtendedClient client, TextWriter console, IBot bot) : base(client, console)
        {
            TwitchClient.OnLeftChannel += TwitchClient_OnLeftChannel;
            Bot = bot;
        }

        private void TwitchClient_OnLeftChannel(Object sender, OnLeftChannelArgs e)
        {
            Console.WriteLine($"Left channel {e.Channel}");

            if (e.Channel == Bot.PrimaryChannelName)
            {
                Bot.PrimaryChannelName = TwitchClient.JoinedChannels.FirstOrDefault()?.Channel ?? null;
                Console.WriteLine($"Primary channel updated to {Bot.PrimaryChannelName}");
            }

        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (arguments.Count() == 0) { return; }

            if (!TwitchClient.IsConnected)
            {
                TwitchClient.Connect();
            }

            String channelName = arguments.FirstOrDefault();

            // If the user inputs any second argument, it will join that channel and not leave the existing channel.

            TwitchClient.LeaveChannel(channelName);
        }

        public override string GetHelp()
        {
            return @"
Leave a twitch channel.

usage: leave <channel name>

    <channel name>                      Name of the twitch channel to leave.
";
        }
    }
}
