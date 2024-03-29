﻿using Chubberino.Bots.Channel.Modules.CheeseGame.Ranks;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Infrastructure.Client.TwitchClients;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame;

public static class IApplicationContextExtensions
{
    public static Player GetPlayer(this IApplicationContext source, ITwitchClientManager client, ChatMessage message)
    {
        var player = source.Players.FirstOrDefault(x => x.TwitchUserID == message.UserId);

        if (player is null)
        {
            player = new Player()
            {
                TwitchUserID = message.UserId,
                Name = message.DisplayName
            }
            .ResetRank();

            source.Add(player);

            source.SaveChanges();

            client.SpoolMessageAsMe(message.Channel, player,
                $"StinkyGlitch NEW CHEESE FACTORY StinkyGlitch " +
                $"You have just started building your own cheese factory where {player.ID - 1} others already reside. " +
                $"Begin producing cheese with \"!cheese\" and spend it at the shop with \"!cheese shop\". " +
                $"You can get help with \"!cheese help\" to see other commands. " +
                $"Good luck!");
        }

        return player;
    }
}