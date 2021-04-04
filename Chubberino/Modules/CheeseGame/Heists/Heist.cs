﻿using Chubberino.Client;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public sealed class Heist : IHeist
    {
        public const String FailToJoinHeistMessage = "You must wager a positive number of cheese to join the heist.";
        public const String SucceedToUpdateHeistMessage = "You update your heist wager to {0} cheese.";
        public const String SucceedToJoinHeistMessage = "You join the heist, wagering {0} cheese.";
        public const String SucceedToLeaveHeistMessage = "You left the heist, and are refunded your {0} cheese.";
        public const Double MinimumWinnerPercent = 0.33;
        public const Double MaximumWinnerPercent = 1;

        public IList<Wager> Wagers { get; }

        public Heist(
            ChatMessage message,
            IApplicationContext context,
            Random random,
            ITwitchClientManager client)
        {
            Wagers = new List<Wager>();
            InitiatorMessage = message;
            InitiatorName = message.DisplayName;
            Context = context;
            Random = random;
            TwitchClient = client;
        }

        public ChatMessage InitiatorMessage { get; }
        public IApplicationContext Context { get; }
        public Random Random { get; }
        public ITwitchClientManager TwitchClient { get; }

        public String InitiatorName { get; }

        public Boolean Start()
        {
            if (Wagers.Count == 0)
            {
                TwitchClient.SpoolMessageAsMe(InitiatorMessage.Channel, "[Heist] No one joined the heist.");
                return false;
            }

            String people = Wagers.Count switch
            {
                0 => "No one goes",
                1 => "1 person goes",
                _ => $"{Wagers.Count} people go"
            };

            var intro = new StringBuilder($"[Heist] {people} into the lair of the great cheese dragon. ");

            Double winnerPercent = Random.NextDouble(MinimumWinnerPercent, MaximumWinnerPercent);

            // Convert.ToInt32 will round up to the nearest Int32 instead of truncating with casting,
            // so a single wager will still have a chance to fail or succeed randomly.
            Int32 winnerCount = Convert.ToInt32(winnerPercent * Wagers.Count);


            if (winnerCount == 0)
            {
                intro.Append("Unfortunately the cheese dragon prevented anyone from getting anything");
                TwitchClient.SpoolMessageAsMe(InitiatorMessage.Channel, intro.ToString());
                return true;
            }


            if (winnerCount == Wagers.Count)
            {
                intro.Append("Everyone made it out with the spoils! ");
            }
            else if (winnerCount == 1)
            {
                intro.Append("One made it out with the spoils! ");
            }
            else
            {
                intro.Append("Some made it out with the spoils! ");
            }

            var winners = new List<Wager>();

            winnerCount.Repeat(() => winners.Add(Random.RemoveElement(Wagers)));

            foreach (Wager wager in winners)
            {
                var player = Context.Players.FirstOrDefault(x => x.TwitchUserID == wager.PlayerTwitchID);
                Int32 winnerPoints = (Int32)((1.0 / winnerPercent + 0.5) * wager.WageredPoints);
                player.AddPoints(winnerPoints);
                Context.SaveChanges();
                intro.Append($"{player.Name} (+{winnerPoints}) ");
            }

            TwitchClient.SpoolMessageAsMe(InitiatorMessage.Channel, intro.ToString());

            return true;
        }

        public void UpdateWager(Player player, Int32 points, Boolean silent = false)
        {
            String updateMessage;

            if (Wagers.TryGetFirst(x => x.PlayerTwitchID == player.TwitchUserID, out var wager))
            {
                // Already in the heist and updating.

                // Refund points.
                player.AddPoints(wager.WageredPoints);

                // Updated so that points is never greater than what the player has.
                Int32 updatedPoints = Math.Min(player.Points, points);

                if (updatedPoints <= 0)
                {
                    Wagers.Remove(wager);
                    updateMessage = String.Format(SucceedToLeaveHeistMessage, wager.WageredPoints);
                }
                else
                {
                    wager.WageredPoints = updatedPoints;
                    player.AddPoints(-updatedPoints);
                    updateMessage = String.Format(SucceedToUpdateHeistMessage, updatedPoints);
                }
                Context.SaveChanges();

            }
            else if (points <= 0)
            {
                // Trying to join the heist, but failing.
                updateMessage = FailToJoinHeistMessage;
            }
            else
            {
                // Joining the heist for the first time.
                Int32 updatedPoints = Math.Min(player.Points, points);
                Wagers.Add(new Wager(player.TwitchUserID, updatedPoints));
                player.AddPoints(-updatedPoints);
                Context.SaveChanges();
                updateMessage = String.Format(SucceedToJoinHeistMessage, updatedPoints);
            }

            if (!silent)
            {
                TwitchClient.SpoolMessageAsMe(InitiatorMessage.Channel, player, "[Heist] " + updateMessage);
            }
        }
    }
}
