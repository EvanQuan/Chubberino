using MouseBot.Implementation.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands
{
    public sealed class Copy : Command
    {
        private String UserToMirror { get; set; }

        private String MessageStart { get; }

        private enum CopyMode
        {
            Disabled = 0,
            Normal = 1,
            Mock = 2
        }

        private CopyMode Mode { get; set; }

        public Copy(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!e.ChatMessage.Username.Equals(UserToMirror, StringComparison.OrdinalIgnoreCase)) { return; }

            Spooler.SpoolMessage(e.ChatMessage.Message, Priority.High);
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (arguments.Count() == 0)
            {
                UserToMirror = null;
                Mode = CopyMode.Disabled;
                Console.WriteLine("Copy disabled");
                return;
            }

            UserToMirror = arguments.First();
            Console.WriteLine("Copying user " + UserToMirror);

            if (arguments.Count() > 1)
            {
                switch (arguments.Skip(1).FirstOrDefault()?.ToLower())
                {
                    case "popoga":
                        Mode = CopyMode.Mock;
                        break;
                }
            }
        }
    }
}
