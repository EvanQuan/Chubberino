using System;

namespace Chubberino.Client
{
    public sealed class BotInfo
    {
        /// <summary>
        /// Current channel joined.
        /// </summary>
        public String ChannelName { get; set; }

        /// <summary>
        /// 100 messages in 30 seconds ~1 message per 0.3 seconds.
        /// </summary>
        public TimeSpan ModeratorThrottlingPeriod { get; } = TimeSpan.FromSeconds(0.3);

        /// <summary>
        /// 20 messages in 30 seconds ~1 message per 1.5 second
        /// </summary>
        public TimeSpan RegularThrottlingPeriod { get; } = TimeSpan.FromSeconds(1.5);

        /// <summary>
        /// Is the bot a broadcaster/moderator/VIP?
        /// </summary>
        public Boolean IsModerator { get; set; }

        public static BotInfo Instance { get; } = new BotInfo();

        private BotInfo() { }


    }
}
