using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System;

namespace Chubberino.Client
{
    public static class TwitchClientManagerExtensions
    {
        public static void SpoolMessage(this ITwitchClientManager source, String message)
        {
            source.Client.SpoolMessage(source.PrimaryChannelName, message);
        }

        public static void SpoolMessage(this ITwitchClientManager source, String channelName, String message)
        {
            source.Client.SpoolMessage(channelName, message);
        }

        public static void SpoolMessageAsMe(this ITwitchClientManager source, String channelName, Player player, String message)
        {
            source.Client.SpoolMessage(channelName, $"/me {player.GetDisplayName()} {message}");
        }

        public static void SpoolMessageAsMe(this ITwitchClientManager source, String channelName, String message)
        {
            source.Client.SpoolMessage(channelName, $"/me {message}");
        }
    }
}
