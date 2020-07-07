using System;
using System.Threading;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Implementation.Extensions
{
    internal static class TwitchClientConnectionExtensions
    {
        public static void EnsureJoinedToChannel(this ITwitchClient client, String channelName)
        {
            SpinWait.SpinUntil(() =>
            {
                if (!client.IsConnected)
                {
                    client.Connect();
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return client.IsConnected;
                }
                return true;

            },
            TimeSpan.FromSeconds(30));

            SpinWait.SpinUntil(() =>
            {
                if (client.JoinedChannels.Count == 0)
                {
                    client.JoinChannel(channelName);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return client.JoinedChannels.Count != 0;
                }
                else
                {
                    return true;
                }

            },
            TimeSpan.FromSeconds(30));
        }
    }
}
