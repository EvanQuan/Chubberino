using System;
using System.Linq;
using System.Threading;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Extensions
{
    internal static class TwitchClientConnectionExtensions
    {
        public static Boolean EnsureJoinedToChannel(this ITwitchClient client, String channelName)
        {
            Boolean isConnected = SpinWait.SpinUntil(() =>
            {
                if (!client.IsConnected)
                {
                    client.Connect();
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return client.IsConnected;
                }
                return true;

            },
            TimeSpan.FromSeconds(10));

            if (!isConnected) { return false; }

            Boolean isJoined = SpinWait.SpinUntil(() =>
            {
                if (client.JoinedChannels.Count == 0)
                {
                    client.JoinChannel(channelName);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return client.JoinedChannels.Any(x => x.Channel.Equals(channelName, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    return true;
                }

            },
            TimeSpan.FromSeconds(10));

            return isJoined;
        }
    }
}
