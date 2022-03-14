using System;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Modules.CheeseGame.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame
{
    public static class TwitchClientManagerExtensions
    {
        public static void SpoolMessageAsMe(this ITwitchClientManager source, String channelName, Player player, String message, Priority priority = Priority.Medium)
        {
            source.SpoolMessage(channelName, $"/me {player.GetDisplayName()} {message}", priority);
        }
    }
}
