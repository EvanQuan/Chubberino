using System;
using System.Timers;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;
using TwitchLib.Communication.Events;

namespace MouseBot.Implementation.Commands
{
    public sealed class MessageTimer : IDisposable
    {
        /// <summary>
        /// 20 messages in 30 seconds + buffer
        /// </summary>
        private static TimeSpan RegularInterval { get; } = TimeSpan.FromSeconds(1.6);

        /// <summary>
        /// 100 messages in 30 seconds + buffer
        /// </summary>
        private static TimeSpan VipInterval { get; } = TimeSpan.FromSeconds(0.35);

        public Boolean CanSendNextMessage { get; private set; }

        public TimeSpan Interval
        {
            get => TimeSpan.FromMilliseconds(Timer.Interval);
            set => Timer.Interval = value.TotalMilliseconds;
        }

        private Timer Timer { get; }

        private ITwitchClient TwitchClient { get; }

        public MessageTimer(ITwitchClient client)
        {
            Timer = new Timer(RegularInterval.TotalMilliseconds)
            {
                AutoReset = false
            };

            Timer.Elapsed += Timer_Elapsed;

            TwitchClient = client;

            TwitchClient.OnMessageSent += Client_OnMessageSent;
            TwitchClient.OnMessageThrottled += Client_OnMessageThrottled;
        }

        private void Client_OnMessageThrottled(Object sender, OnMessageThrottledEventArgs e)
        {
            // To be safe. Possibly can remove.
            RestartTimer();
        }

        private void Client_OnMessageSent(Object sender, OnMessageSentArgs e)
        {
            RestartTimer();
        }

        private void RestartTimer()
        {
            CanSendNextMessage = false;
            Timer.Stop();
            Timer.Start();
        }

        private void Timer_Elapsed(Object sender, ElapsedEventArgs e)
        {
            CanSendNextMessage = true;
        }

        public void Dispose()
        {
            Timer.Dispose();
            TwitchClient.OnMessageSent -= Client_OnMessageSent;
            TwitchClient.OnMessageThrottled -= Client_OnMessageThrottled;
        }
    }
}
