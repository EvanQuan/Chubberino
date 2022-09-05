using System;

namespace Chubberino.Infrastructure.Client.TwitchClients;

public static class TwitchClientManagerExtensions
{
    public static void SpoolMessage(this ITwitchClientManager source, String message, Priority priority = Priority.Medium)
    {
        source.SpoolMessage(source.PrimaryChannelName, message, priority);
    }

    public static void SpoolMessageAsMe(this ITwitchClientManager source, String channelName, String message, Priority priority = Priority.Medium)
    {
        source.SpoolMessage(channelName, $"/me {message}", priority);
    }
}
