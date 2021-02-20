using System;
using TwitchLib.Communication.Models;

namespace Chubberino.Client
{
    public sealed class ModeratorClientOptions : ClientOptions, IModeratorClientOptions
    {
        public ModeratorClientOptions()
        {
            MessagesAllowedInPeriod = 90;
            ThrottlingPeriod = TimeSpan.FromSeconds(30);
        }
    }
}
