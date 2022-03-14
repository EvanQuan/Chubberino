using System;
using Chubberino.Common.ValueObjects;
using TwitchLib.Client.Models;

namespace Chubberino.Infrastructure.Credentials
{
    public class LoginCredentials
    {
        public LoginCredentials(ConnectionCredentials connectionCredentials, Boolean isBot, Name primaryChannelName)
        {
            ConnectionCredentials = connectionCredentials;
            IsBot = isBot;
            PrimaryChannelName = primaryChannelName;
        }

        public ConnectionCredentials ConnectionCredentials { get; }

        public Boolean IsBot { get; }

        /// <summary>
        /// Name of the initial primary channel to join.
        /// </summary>
        public Name PrimaryChannelName { get; }
    }
}
