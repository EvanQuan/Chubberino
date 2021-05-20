using Chubberino.Client.Credentials;
using System;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    public interface IBot : IDisposable
    {
        BotState State { get; set; }

        /// <summary>
        /// Twitch user name that the bot is logged into.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Is the bot a broadcaster/moderator/VIP?
        /// </summary>
        Boolean IsModerator { get; set; }

        /// <summary>
        /// Start the bot, logging in as the specified user.
        /// </summary>
        /// <param name="clientOptions"></param>
        /// <param name="credentials">User to log in as. If null, will prompt for which user to log in as.</param>
        /// <returns>The login credentials of the user that successfully logged in; null otherwise.</returns>
        LoginCredentials Start(IClientOptions clientOptions = null, LoginCredentials credentials = null);

        String GetPrompt();

        void Refresh(IClientOptions clientOptions = null);

        void ReadCommand(String command);
    }
}