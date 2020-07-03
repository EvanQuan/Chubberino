using MouseBot.Implementation.Abstractions;
using System;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands
{
    public sealed class Mirror : Command
    {
        private String UserToMirror { get; set; }

        public Mirror(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!e.ChatMessage.Username.Equals(UserToMirror, StringComparison.OrdinalIgnoreCase)) { return; }

            Spooler.SpoolMessage(e.ChatMessage.Message);
        }

        public override void Execute(params String[] arguments)
        {
            if (arguments.Length == 0)
            {
                UserToMirror = null;
                Console.WriteLine("Mirror disabled");
                return;
            }

            UserToMirror = arguments[0];
            Console.WriteLine("Mirroring user " + UserToMirror);
        }
    }
}
