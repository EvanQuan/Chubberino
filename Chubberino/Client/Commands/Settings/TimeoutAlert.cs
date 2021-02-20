using Chubberino.Client.Abstractions;
using System;
using System.IO;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class TimeoutAlert : Setting
    {
        public TimeoutAlert(ITwitchClientManager client, IConsole console)
            : base(client, console)
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

        public void TwitchClient_OnUserTimedout(Object sender, OnUserTimedoutArgs e)
        {
            TwitchClientManager.SpoolMessage($"WideHardo FREE MY MAN {e.UserTimeout.Username.ToUpper()}");
        }
    }
}
