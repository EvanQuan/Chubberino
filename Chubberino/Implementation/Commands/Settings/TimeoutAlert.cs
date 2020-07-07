using Chubberino.Implementation.Abstractions;
using System;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Implementation.Commands.Settings
{
    internal class TimeoutAlert : Setting
    {
        public TimeoutAlert(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnUserTimedout += TwitchClient_OnUserTimedout;
            IsEnabled = true;
        }

        private void TwitchClient_OnUserTimedout(Object sender, TwitchLib.Client.Events.OnUserTimedoutArgs e)
        {
            TwitchClient.SendMessage(e.UserTimeout.Channel, $"WideHardo FREE MY MAN {e.UserTimeout.Username.ToUpper()}");
        }
    }
}
