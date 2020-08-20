using Chubberino.Client.Abstractions;
using System;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    internal class TimeoutAlert : Setting
    {
        public TimeoutAlert(IExtendedClient client)
            : base(client)
        {
            Enable = twitchClient =>
            {
                twitchClient.OnUserTimedout += TwitchClient_OnUserTimedout;
            };

            Disable = twitchClient =>
            {
                twitchClient.OnUserTimedout -= TwitchClient_OnUserTimedout;
            };
        }

        private void TwitchClient_OnUserTimedout(Object sender, TwitchLib.Client.Events.OnUserTimedoutArgs e)
        {
            TwitchClient.SendMessage(e.UserTimeout.Channel, $"WideHardo FREE MY MAN {e.UserTimeout.Username.ToUpper()}");
        }
    }
}
