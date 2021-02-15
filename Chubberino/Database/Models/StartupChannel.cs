using System;

namespace Chubberino.Database.Models
{
    /// <summary>
    /// Channel to join at program startup.
    /// </summary>
    public class StartupChannel
    {
        public Int32 ID { get; }

        /// <summary>
        /// Twitch user ID.
        /// </summary>
        public String UserID { get; }
    }
}
