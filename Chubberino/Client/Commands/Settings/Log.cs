using System;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class Log : Setting
    {
        public Log(ITwitchClientManager client, IConsole console)
            : base(client, console)
        {
            Enable = twitchClient =>
            {
                twitchClient.OnLog += TwitchClient_OnLog;
                twitchClient.OnRitualNewChatter += TwitchClient_OnRitualNewChatter;
            };
            Disable = twitchClient =>
            {
                twitchClient.OnLog -= TwitchClient_OnLog;
            };
        }

        private void TwitchClient_OnRitualNewChatter(Object sender, OnRitualNewChatterArgs e)
        {
        }

        public void TwitchClient_OnLog(Object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
        }
    }
}
