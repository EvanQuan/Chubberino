using System;

namespace Chubberino.Database.Models
{
    public class UserCredentials
    {
        public Int32 ID { get; set; }

        /// <summary>
        /// Twitch username of the acount to log in as.
        /// </summary>
        public String TwitchUsername { get; set; }

        /// <summary>
        /// The OAuth token to log into the Twitch account.
        /// </summary>
        /// <remarks>
        /// Learn more: https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#oauth-client-credentials-flow
        /// Easy way to generate: https://twitchtokengenerator.com/
        /// </remarks>
        public String AccessToken { get; set; }

        /// <summary>
        /// Indicates the user is a bot.
        /// </summary>
        public Boolean IsBot { get; set; }
    }
}
