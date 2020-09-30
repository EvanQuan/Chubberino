using Chubberino.Client.Abstractions;
using System;
using System.IO;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class Log : Setting
    {
        public Log(IExtendedClient client, TextWriter console)
            : base(client, console)
        {
            Enable = twitchClient =>
            {
                twitchClient.OnLog += TwitchClient_OnLog;
            };
            Disable = twitchClient =>
            {
                twitchClient.OnLog -= TwitchClient_OnLog;
            };
        }

        public void TwitchClient_OnLog(Object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
        }
    }
}
