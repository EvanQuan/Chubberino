using Chubberino.Client.Abstractions;
using System;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class YepKyle : Setting
    {
        public YepKyle(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
        }

        private void TwitchClient_OnMessageReceived(Object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            if (!IsEnabled) { return; }
            if (e.ChatMessage.Username.Equals("YepKyle", StringComparison.OrdinalIgnoreCase))
            {
                Spooler.SpoolMessage($"YEP KYLE");
            }
        }
    }
}
