using System;
using TwitchLib.Communication.Interfaces;

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
        public IClientOptions ModeratorClientOptions { get; }
        /// <summary>
        /// 20 messages in 30 seconds ~1 message per 1.5 second
        /// </summary>
        public IClientOptions RegularClientOptions { get; }
        /// <summary>
        /// Is the bot a broadcaster/moderator/VIP?
        /// </summary>
        public Boolean IsModerator { get; set; }

        public BotInfo(IClientOptions moderatorOptions, IClientOptions regularOptions)
        {
            ModeratorClientOptions = moderatorOptions;
            RegularClientOptions = regularOptions;
        }
    }
}
