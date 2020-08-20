namespace Chubberino.Client
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

        /// <summary>
        /// The bot should restart, reconnecting with the original settings.
        /// </summary>
        ShouldRestart = 2,
    }
}
