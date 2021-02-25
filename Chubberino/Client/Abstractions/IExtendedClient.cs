using System;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Abstractions
{
    public interface IExtendedClient : ITwitchClient
    {
        void SpoolMessage(String channelName, String message);

        Boolean EnsureJoinedToChannel(String channel);
    }
}
