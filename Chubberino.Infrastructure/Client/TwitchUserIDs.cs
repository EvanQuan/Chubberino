using System;
using System.Collections.Generic;

namespace Chubberino.Infrastructure.Client
{
    /// <summary>
    /// Notable twitch user IDs.
    /// </summary>
    public static class TwitchUserIDs
    {
        public static IReadOnlyCollection<String> ChannelBots { get; } = new HashSet<String>
        {
            ThePositiveBot,
            MoTroBo,
            Nightbot,
            SimonSaysBot,
            VJBotardo,
            StreamElements,
            AmazefulBot
        };

        /// <summary>
        /// ThePositiveBot
        /// </summary>
        public const String ThePositiveBot = "425363834";

        /// <summary>
        /// Moya translation bot
        /// </summary>
        public const String MoTroBo = "610128514";

        /// <summary>
        /// Nightbot
        /// </summary>
        public const String Nightbot = "19264788";

        /// <summary>
        /// SimonSaysBot
        /// </summary>
        public const String SimonSaysBot = "469718952";

        /// <summary>
        /// VJBotardo
        /// </summary>
        public const String VJBotardo = "500670723";

        /// <summary>
        /// StreamElements
        /// </summary>
        public const String StreamElements = "100135110";

        /// <summary>
        /// AmazefulBot
        /// </summary>
        public const String AmazefulBot = "496470032";
    }
}
