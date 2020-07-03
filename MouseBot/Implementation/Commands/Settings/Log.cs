using MouseBot.Implementation.Abstractions;
using System;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands.Settings
{
    internal sealed class Log : Setting
    {
        public Log(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnLog += TwitchClient_OnLog;
        }

        private void TwitchClient_OnLog(Object sender, TwitchLib.Client.Events.OnLogArgs e)
        {
            if (IsEnabled)
            {
                Console.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
            }
        }
    }
}
