using System;
using TwitchLib.Communication.Models;

namespace Chubberino.Infrastructure.Client;

public sealed class RegularClientOptions : ClientOptions, IRegularClientOptions
{
    public RegularClientOptions()
    {
        MessagesAllowedInPeriod = 20;
        ThrottlingPeriod = TimeSpan.FromSeconds(30);
    }
}
