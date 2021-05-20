using Chubberino.Client.Credentials;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    public interface ITwitchClientManager
    {
        String PrimaryChannelName { get; set; }

        Boolean IsBot { get; }

        ITwitchClient Client { get; }


        /// <summary>
        /// Try to log in as a user.
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="clientOptions"></param>
        /// <param name="credentials">Supplied credentials; will prompt for credentials if null.</param>
        /// <returns>the user credentials of the user successfully logged in; otherwise null.</returns>
        LoginCredentials TryInitialize(IBot bot, IClientOptions clientOptions = null, LoginCredentials credentials = null);

        Boolean TryJoinInitialChannels(IReadOnlyList<JoinedChannel> previouslyJoinedChannels = null);

        void SpoolMessage(String channelName, String message, Priority priority = Priority.Medium);

        Boolean EnsureJoinedToChannel(String channel);
    }
}
