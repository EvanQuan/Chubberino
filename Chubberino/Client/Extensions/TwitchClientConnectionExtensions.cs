using Chubberino.Client.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
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
            TimeSpan.FromSeconds(30));

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
            TimeSpan.FromSeconds(30));

            Boolean channelNameUpdated = SpinWait.SpinUntil(() =>
            {
                return BotInfo.Instance.ChannelName.Equals(channelName, StringComparison.OrdinalIgnoreCase);
            },
            TimeSpan.FromSeconds(30));

            return isConnected && isJoined && channelNameUpdated;
        }
    }
}
