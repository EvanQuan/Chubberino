using System;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.TwitchLibExtensions
{
    internal static class TwitchClientConnectionExtensions
    {
        public static void EnsureJoinedToChannel(this ITwitchClient client, String channelName)
        {
            if (!client.IsConnected)
            {
                client.Connect();
            }

            if (client.JoinedChannels.Count == 0)
            {
                client.JoinChannel(channelName);
            }
        }
    }
}
