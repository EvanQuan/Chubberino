using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client.Abstractions
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

        Boolean Start(IClientOptions clientOptions = null, Boolean askForCredentials = true);

        String GetPrompt();

        void Refresh(IClientOptions clientOptions = null, Boolean askForCredentials = true);

        void ReadCommand(String command);
    }
}