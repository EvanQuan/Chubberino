using System;
using System.Collections.Concurrent;
using System.Timers;

namespace MouseBot.Implementation
{
    [Obsolete("Use MessageTimer")]
    public sealed class TimedQueue : IDisposable
    {
        /// <summary>
        /// 20 messages in 30 seconds + buffer
        /// </summary>
        public static TimeSpan RegularInterval { get; } = TimeSpan.FromSeconds(1.6);

        /// <summary>
        /// 100 messages in 30 seconds + buffer
        /// </summary>
        public static TimeSpan VipInterval { get; } = TimeSpan.FromSeconds(0.35);

        private Timer Timer { get; }

        private ConcurrentQueue<String> QueuedMessages { get; } = new ConcurrentQueue<String>();

        private Boolean ShouldDequeue { get; set; } = true;

        public TimeSpan Interval
        {
            get => TimeSpan.FromMilliseconds(Timer.Interval);
            set => Timer.Interval = value.TotalMilliseconds;
        }

        public Int32 Count => QueuedMessages.Count;

        public TimedQueue()
        {
            Timer = new Timer(RegularInterval.TotalMilliseconds)
            {
                AutoReset = false
            };

            Timer.Elapsed += OnElapsed;
        }

        internal void Clear()
        {
            Timer.Stop();
            Timer.Start();
            QueuedMessages.Clear();
        }

        private void OnElapsed(Object sender, ElapsedEventArgs e)
        {
            ShouldDequeue = true;
        }

        public Boolean ShouldWait { get; }

        public void Enqueue(String message)
        {
            QueuedMessages.Enqueue(message);
        }

        public Boolean TryDequeue(out String message)
        {
            if (ShouldDequeue)
            {
                if (QueuedMessages.TryDequeue(out message))
                {
                    Timer.Stop();
                    Timer.Start();
                    ShouldDequeue = false;
                    return true;
                }
            }

            message = null;
            return false;
        }

        public void Dispose()
        {
            Timer.Dispose();
        }
    }
}
