using System;
using TwitchLib.Communication.Models;

namespace Chubberino.Client
{
    public sealed class RegularClientOptions : ClientOptions, IRegularClientOptions
    {
        public RegularClientOptions()
        {
            MessagesAllowedInPeriod = 20;
            ThrottlingPeriod = TimeSpan.FromSeconds(30);
        }
    }
}
