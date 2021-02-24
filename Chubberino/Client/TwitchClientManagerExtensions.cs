using System;

namespace Chubberino.Client
{
    public static class TwitchClientManagerExtensions
    {
        public static void SpoolMessage(this ITwitchClientManager source, String message)
        {
            source.Client.SpoolMessage(source.PrimaryChannelName, message);
        }
    }
}
