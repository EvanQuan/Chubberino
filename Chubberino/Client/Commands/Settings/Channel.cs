using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class Channel : Setting
    {
        public override String Status => $"\n\tprimary: {Bot.PrimaryChannelName}"
            + $"\n\tOthers:"
            + "\n\t\t" + String.Join("\n\t\t", TwitchClient.JoinedChannels.Where(x => x.Channel != Bot.PrimaryChannelName).Select(x => x.Channel));

        private IBot Bot { get; }

        public Channel(IExtendedClient client, TextWriter console, IBot bot) : base(client, console)
        {
            Bot = bot;
            IsEnabled = true;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
        }

        public override Boolean Set(String property, IEnumerable<String> arguments)
        {
            var primaryChannelName = arguments.FirstOrDefault();

            if (primaryChannelName is null) { return false; }

            switch (property?.ToLower())
            {
                case "p":
                case "primary":
                    var channel = TwitchClient.JoinedChannels
                        .FirstOrDefault(x => x.Channel.Equals(primaryChannelName, StringComparison.OrdinalIgnoreCase))
                        ?.Channel ?? null;

                    if (channel != null)
                    {
                        Bot.PrimaryChannelName = channel;
                        return true;
                    }
                    return false;
            }
            return false;
        }
    }
}
