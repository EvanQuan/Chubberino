using System;

namespace Chubberino.Database.Models
{
    public class UserCredentials
    {
        public Int32 ID { get; set; }

        public String TwitchUsername { get; set; }

        public String AccessToken { get; set; }
    }
}
