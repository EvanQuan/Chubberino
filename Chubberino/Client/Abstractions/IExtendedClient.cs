using System;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Abstractions
{
    public interface IExtendedClient : ITwitchClient, IMessageSpooler
    {
        Boolean EnsureJoinedToChannel(String channel);
    }
}
