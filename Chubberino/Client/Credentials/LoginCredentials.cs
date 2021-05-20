using System;
using TwitchLib.Client.Models;

namespace Chubberino.Client.Credentials
{
    public class LoginCredentials
    {
        public LoginCredentials(ConnectionCredentials connectionCredentials, Boolean isBot)
        {
            ConnectionCredentials = connectionCredentials;
            IsBot = isBot;
        }

        public ConnectionCredentials ConnectionCredentials { get; }

        public Boolean IsBot { get; }
    }
}
