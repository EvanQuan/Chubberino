using Chubberino.Client.Abstractions;
using System;
using System.IO;

namespace Chubberino.Client.Commands.Settings
{
    internal sealed class Log : Setting
    {
        public Log(IExtendedClient client, TextWriter console)
            : base(client, console)
        {
            Enable = twitchClient =>
            {
                twitchClient.OnLog += TwitchClient_OnLog;
            };
        }

        private void TwitchClient_OnLog(Object sender, TwitchLib.Client.Events.OnLogArgs e)
        {
            if (!IsEnabled) { return; }

            Console.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
        }
    }
}
