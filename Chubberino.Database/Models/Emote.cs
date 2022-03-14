using System;

namespace Chubberino.Database.Models
{
    public class Emote
    {
        public Int32 ID { get; set; }

        /// <summary>
        /// Emote name.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Twitch display name.
        /// </summary>
        public String TwitchDisplayName { get; set; }

        /// <summary>
        /// The Twitch user ID of the user that has the emote enabled.
        /// </summary>
        public String TwitchUserID { get; set; }

        /// <summary>
        /// Emote category.
        /// </summary>
        public EmoteCategory Category { get; set; }
    }
}
