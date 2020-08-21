using System;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Communication.Models;

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
        public IClientOptions ModeratorClientOptions { get; } = new ClientOptions()
        {
            MessagesAllowedInPeriod = 100,
            ThrottlingPeriod = TimeSpan.FromSeconds(30)
        };

        /// <summary>
        /// 20 messages in 30 seconds ~1 message per 1.5 second
        /// </summary>
        public IClientOptions RegularClientOptions { get; } = new ClientOptions()
        {
            MessagesAllowedInPeriod = 20,
            ThrottlingPeriod = TimeSpan.FromSeconds(30)
        };

        /// <summary>
        /// Is the bot a broadcaster/moderator/VIP?
        /// </summary>
        public Boolean IsModerator { get; set; }

        public static BotInfo Instance { get; } = new BotInfo();

        private BotInfo() { }
    }
}
