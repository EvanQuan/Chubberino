using Chubberino.Client.Abstractions;
using Chubberino.Client.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class AutoPogO : Setting
    {
        private HashSet<String> UsersToPogO { get; set; }

        public override String Status => base.Status
            + $"\n\tUsers:"
            + $"\n\t\t{UsersToPogO.ToLineDelimitedString(2)}";

        public AutoPogO(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            UsersToPogO = new HashSet<String>();
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
        }

        private void TwitchClient_OnMessageReceived(Object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            if (!IsEnabled) { return; }

            if (UsersToPogO.Contains(e.ChatMessage.Username))
            {
                Spooler.SpoolMessage($"@{e.ChatMessage.Username} PogO");
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
