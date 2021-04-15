using System;

namespace Chubberino.Database.Models
{
    public class Emote
    {
        public Int32 ID { get; set; }

        public String Name { get; }

        /// <summary>
        /// The Twitch user ID of the user that has the emote enabled.
        /// </summary>
        public String TwitchUserID { get; set; }

        public EmoteCategory Category { get; set; }
    }
}
