using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Interfaces;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public sealed class AtAll : UserCommand
    {
        private ITwitchAPI Api { get; }

        public AtAll(ITwitchClientManager client, IConsole console, ITwitchAPI api) : base(client, console)
        {
            IsEnabled = true;
            Api = api;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            Char userTypeString = arguments.FirstOrDefault()?.ToLower()[0] ?? default;

            var userType = userTypeString switch
            {
                'm' => UserType.Moderator,
                'v' => UserType.VIP,
                _ => UserType.Viewer,
            };

            // Remove user type parameter from message.
            if (userType != UserType.Viewer)
            {
                arguments = arguments.Skip(1);
            }

            var chatters = Api.Undocumented.GetChattersAsync(TwitchClientManager.PrimaryChannelName).Result
                .Where(user => user.UserType >= userType);

            var message = " " + String.Join(' ', arguments);

            foreach (var user in chatters)
            {
                TwitchClientManager.Client.SpoolMessage(TwitchClientManager.PrimaryChannelName, user.Username + message);
            };
        }

        public override string GetHelp()
        {
            return @"
@'s all chatters in the channel.

usage: atall [user type] <message>

[user type]     - [m]ods and staff
                - [v]ips, mods and staff
                - Defaults to all users
";
        }
    }
}
