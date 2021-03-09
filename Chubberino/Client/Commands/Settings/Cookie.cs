using Chubberino.Client.Abstractions;
using Chubberino.Client.Services;
using Chubberino.Utility;
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
        private IDateTimeService DateTime { get; }
        private Boolean Responded { get; set; }

        public String Channel { get; private set; }

        public DateTime LastCookieTime { get; private set; }

        public override String Status => base.Status
            + $"\n\tChannel: {Channel}"
            + $"\n\tLast cookie: {LastCookieTime}";

        public Cookie(
            ITwitchClientManager client,
            IBot bot,
            IRepeater repeater,
            ISpinWait spinWait,
            IConsole console,
            IDateTimeService dateTime)
            : base(client, console)
        {
            Bot = bot;
            Repeater = repeater;
            SpinWait = spinWait;
            DateTime = dateTime;
            Repeater.Action = SpoolRepeatMessages;
            Repeater.Interval = TimeSpan.FromHours(2);
            Channel = client.PrimaryChannelName;

            Enable = twitchClient => twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            Disable = twitchClient => twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
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
                TwitchClientManager.SpoolMessage(Channel, "!cookie");
                SpinWait.SpinUntil(() => Responded, TimeSpan.FromSeconds(5));
            }
            LastCookieTime = DateTime.Now;
            Responded = false;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            base.Execute(arguments);

            Repeater.IsRunning = IsEnabled;
        }

        public override Boolean Set(String property, IEnumerable<String> arguments)
        {
            switch (property?.ToLower())
            {
                case "c":
                case "channel":
                    if (arguments.TryGetFirst(out String channel))
                    {
                        Channel = channel;
                        return true;
                    }
                    return false;
            }
            return false;
        }
    }
}
