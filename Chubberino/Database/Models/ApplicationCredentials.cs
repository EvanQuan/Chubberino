using System;

namespace Chubberino.Database.Models
{
    public class ApplicationCredentials
    {
        public Int32 ID { get; set; }

        public String InitialTwitchPrimaryChannelName { get; set; }

        public String TwitchAPIClientID { get; set; }

        public String WolframAlphaAppID { get; set; }
    }
}
