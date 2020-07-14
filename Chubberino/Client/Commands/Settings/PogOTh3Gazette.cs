using Chubberino.Client.Abstractions;
using System;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class PogOTh3Gazette : Setting
    {
        public PogOTh3Gazette(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            IsEnabled = true;
        }

        private void TwitchClient_OnMessageReceived(Object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            if (!IsEnabled) { return; }
            if (e.ChatMessage.Username.Equals("Th3Gazette", StringComparison.OrdinalIgnoreCase))
            {
                Spooler.SpoolMessage($"@{e.ChatMessage.Username} PogO");
            }
        }
    }
}
