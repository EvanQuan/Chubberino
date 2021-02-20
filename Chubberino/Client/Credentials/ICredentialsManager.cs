using Chubberino.Database.Models;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Client.Credentials
{
    public interface ICredentialsManager
    {
        Boolean TryGetConnectionCredentials(out ConnectionCredentials credentials);
    }
}
