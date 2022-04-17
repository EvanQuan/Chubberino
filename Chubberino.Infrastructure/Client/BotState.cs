using System;

namespace Chubberino.Infrastructure.Client
{
    /// <summary>
    /// The state that the bot is in.
    /// </summary>
    public enum BotState
    {
        /// <summary>
        /// The bot should continue to run, meaning the user should be
        /// reprompted for another command.
        /// </summary>
        ShouldContinue = 0,

        /// <summary>
        /// The program should stop.
        /// </summary>
        ShouldStop = 1,
    }
}
