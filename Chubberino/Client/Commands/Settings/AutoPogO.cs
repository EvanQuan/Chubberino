using Chubberino.Client.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class AutoPogO : Setting
    {
        private HashSet<String> UsersToPogO { get; set; }

        public override String Status => base.Status
            + $"\n\tUsers:"
            + $"\n\t\t{UsersToPogO.ToLineDelimitedString(2)}";

        public AutoPogO(ITwitchClientManager client, IConsole console)
            : base(client, console)
        {
            UsersToPogO = new HashSet<String>();
            Enable = twitchClient =>
            {
                twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            };
            Disable = twitchClient =>
            {
                twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
            };
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (UsersToPogO.Contains(e.ChatMessage.Username))
            {
                TwitchClientManager.SpoolMessage($"@{e.ChatMessage.DisplayName} PogO");
            }
        }

        public override Boolean Set(String property, IEnumerable<String> arguments)
        {
            switch (property)
            {
                case "u":
                case "user":
                case "users":
                    UsersToPogO.Clear();
                    String userToSet = arguments.FirstOrDefault();
                    if (userToSet != null)
                    {
                        UsersToPogO.Add(userToSet);
                    }
                    return UsersToPogO.Count == 1;
                default:
                    return false;
            }
        }

        public override Boolean Add(String property, IEnumerable<String> arguments)
        {
            switch (property)
            {
                case "u":
                case "user":
                case "users":
                    Int32 beforeCount = UsersToPogO.Count;
                    foreach (String username in arguments)
                    {
                        UsersToPogO.Add(username);
                    }
                    return beforeCount != UsersToPogO.Count;
                default:
                    return false;
            }
        }

        public override Boolean Remove(String property, IEnumerable<String> arguments)
        {
            switch (property)
            {
                case "u":
                case "user":
                    Int32 beforeCount = UsersToPogO.Count;
                    foreach (String username in arguments)
                    {
                        UsersToPogO.Remove(username);
                    }
                    return beforeCount != UsersToPogO.Count;
                default:
                    return false;
            }
        }

    }
}
