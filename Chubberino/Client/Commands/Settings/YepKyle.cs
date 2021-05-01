using System;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class YepKyle : Setting
    {
        public YepKyle(ITwitchClientManager client, IConsole console)
            : base(client, console)
        {
            Enable = twitchClient =>
            {
                twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            };

            Disable = twitchClient =>
            {
                twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
            };
        }

        private void TwitchClient_OnMessageReceived(Object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Username.Equals("YepKyle", StringComparison.OrdinalIgnoreCase))
            {
                TwitchClientManager.SpoolMessage($"YEP KYLE");
            }
        }
    }
}
