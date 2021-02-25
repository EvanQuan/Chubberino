using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class Cookie : Setting
    {
        public IBot Bot { get; }
        private IRepeater Repeater { get; }
        public ISpinWait SpinWait { get; }
        private Boolean Responded { get; set; }

        public Cookie(ITwitchClientManager client, IBot bot, IRepeater repeater, ISpinWait spinWait, IConsole console) : base(client, console)
        {
            Bot = bot;
            Repeater = repeater;
            SpinWait = spinWait;
            Repeater.Action = SpoolRepeatMessages;
            Repeater.Interval = TimeSpan.FromHours(2);

            Enable = twitchClient => twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.UserId == TwitchUserIDs.ThePositiveBot)
            {
                if (e.ChatMessage.Message.Contains(Bot.Name, StringComparison.OrdinalIgnoreCase))
                {
                    Responded = true;
                }
            }
        }

        private void SpoolRepeatMessages()
        {
            while (!Responded)
            {
                TwitchClientManager.SpoolMessage("!cookie");
                SpinWait.SpinUntil(() => Responded, TimeSpan.FromSeconds(5));
            }

            Responded = false;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            base.Execute(arguments);

            Repeater.IsRunning = IsEnabled;
        }
    }
}
