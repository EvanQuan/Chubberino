﻿using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame;
using Chubberino.Bots.Channel.Modules.CheeseGame.Points;
using Chubberino.Common.Extensions;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Heists;

public sealed class Heist : IHeist
{
    public const String FailToJoinHeistMessage = "You must wager a positive number of cheese to join the heist.";
    public const String FailToJoinHeistBecauseNoCheeseMessage = "You do not have any cheese to wager to join the heist.";
    public const String SucceedToUpdateHeistMessage = "You update your heist wager from {0} to {1} cheese.";
    public const String WagerIsUnchangedMessage = "Your heist wager is unchanged, at {0} cheese.";
    public const String SucceedToJoinHeistMessage = "You join the heist, wagering {0} cheese.";
    public const String SucceedToLeaveHeistMessage = "You leave the heist, and are refunded your {0} cheese.";

    public IList<Wager> Wagers { get; }

    public Heist(
        ChatMessage message,
        Random random,
        ITwitchClientManager client)
    {
        Wagers = new List<Wager>();
        InitiatorMessage = message;
        Random = random;
        TwitchClient = client;
    }

    public ChatMessage InitiatorMessage { get; }

    public Random Random { get; }

    public ITwitchClientManager TwitchClient { get; }

    public Boolean Start(IApplicationContext context)
    {
        if (Wagers.Count == 0)
        {
            TwitchClient.SpoolMessageAsMe(InitiatorMessage.Channel,
                "[Heist] No one joined the heist.");
            return false;
        }

        var intro = new StringBuilder("[Heist] ");

        intro.Append(Wagers.Count switch
        {
            0 => "No one goes",
            1 => "1 person goes",
            _ => $"{Wagers.Count} people go"
        });

        intro.Append(" into the lair of the great cheese dragon. ");

        Double winnerPercent = Random.NextDouble(0, 1.4).Min(1);

        // Convert.ToInt32 will round up to the nearest Int32 instead of truncating with casting,
        // so a single wager will still have a chance to fail or succeed randomly.
        Int32 winnerCount = Convert.ToInt32(winnerPercent * Wagers.Count);


        if (winnerCount == 0)
        {
            intro.Append(
                "Unfortunately the cheese dragon prevented anyone from getting anything.");
            TwitchClient.SpoolMessageAsMe(InitiatorMessage.Channel, intro.ToString());
            return true;
        }

        var result = winnerCount >= Wagers.Count
            ? "Everyone made it out with the spoils! "
            : winnerCount + " made it out with the spoils! ";

        intro.Append(result);

        var winners = new List<Wager>();

        winnerCount.Repeat(() =>
        {
            var result = Random.RemoveElement(Wagers);
            result.IfSome(wager => winners.Add(wager));
        });

        foreach (Wager wager in winners)
        {
            var player = context
                .Players
                .FirstOrDefault(x => x.TwitchUserID == wager.PlayerTwitchID);

            Int32 winnerPoints = (Int32)((1.0 / winnerPercent + 0.5)* wager.WageredPoints).Max(2);
            player.AddPoints(winnerPoints);
            context.SaveChanges();
            intro.Append($"{player.Name} (+{winnerPoints}) ");
        }

        TwitchClient.SpoolMessageAsMe(InitiatorMessage.Channel, intro.ToString());

        return true;
    }

    public void UpdateWager(
        IApplicationContext context,
        Player player,
        Func<Player, Int32> points,
        Boolean silent = false)
    {
        var updateMessage = new StringBuilder("[Heist] ");

        Priority priority = Priority.Medium;

        Wagers.TryGetFirst(x => x.PlayerTwitchID == player.TwitchUserID)
            .Some(wager =>
            {
                // Already in the heist and updating.

                // Refund points.
                player.AddPoints(wager.WageredPoints);

                // Updated so that points is never greater than what the player has.
                Int32 updatedPoints = Math.Min(player.Points, points(player));

                if (updatedPoints <= 0)
                {
                    Wagers.Remove(wager);
                    updateMessage.Append(SucceedToLeaveHeistMessage.Format(wager.WageredPoints));
                }
                else
                {
                    Int32 oldWager = wager.WageredPoints;
                    wager.WageredPoints = updatedPoints;
                    player.AddPoints(-updatedPoints);

                    if (oldWager == updatedPoints)
                    {
                        updateMessage.Append(WagerIsUnchangedMessage.Format(oldWager));
                        priority = Priority.Low;
                    }
                    else
                    {
                        updateMessage.Append(SucceedToUpdateHeistMessage.Format(oldWager, updatedPoints));
                    }
                }
                context.SaveChanges();
            })
            .None(() =>
            {
                if (player.Points == 0)
                {
                    updateMessage.Append(FailToJoinHeistBecauseNoCheeseMessage);
                    priority = Priority.Low;
                }
                else if (points(player) <= 0)
                {
                    // Trying to join the heist, but failing.
                    updateMessage.Append(FailToJoinHeistMessage);
                    priority = Priority.Low;
                }
                else
                {
                    // Joining the heist for the first time.
                    Int32 updatedPoints = Math.Min(player.Points, points(player));
                    Wagers.Add(new Wager()
                    {
                        PlayerTwitchID = player.TwitchUserID,
                        WageredPoints = updatedPoints
                    });
                    player.AddPoints(-updatedPoints);
                    context.SaveChanges();
                    updateMessage.Append(SucceedToJoinHeistMessage.Format(updatedPoints));
                }
            });

        if (!silent)
        {
            TwitchClient.SpoolMessageAsMe(InitiatorMessage.Channel, player, updateMessage.ToString(), priority);
        }
    }
}