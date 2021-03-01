using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Utility;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Rankings
{
    public class RankManager : AbstractCommandStrategy, IRankManager
    {
        private IReadOnlyDictionary<Rank, Int32> PointsToRank { get; } = new Dictionary<Rank, Int32>()
        {
            {  Rank.Bronze, 200 },
            {  Rank.Silver, 400 },
            {  Rank.Gold, 800 },
            {  Rank.Platinum, 1600 },
            {  Rank.Diamond, 3200 },
            {  Rank.Master, 6400 },
            {  Rank.Grandmaster, 12800 },
            {  Rank.Legend, 25600 },
        };

        public RankManager(IApplicationContext context, ITwitchClientManager client, Random random, IEmoteManager emoteManager)
            : base(context, client, random, emoteManager)
        {
        }

        public void RankUp(ChatMessage message)
        {
            var player = GetPlayer(message);

            String outputMessage;

            if (PointsToRank.TryGetValue(player.Rank, out Int32 pointsToRank))
            {
                Rank newRank = player.Rank.Next();

                if (player.Points >= pointsToRank)
                {
                    if (newRank == Rank.Bronze)
                    {
                        // Prestige instead of rank up
                        player.Points -= pointsToRank;
                        player.ResetRank();
                        player.Prestige++;
                        Context.SaveChanges();
                        outputMessage = $"You prestiged back to {Rank.Bronze} and have gained a permanent {(Int32)(Constants.PrestigeBonus * 100)}% cheese gain boost.";
                    }
                    else
                    {
                        player.Points -= pointsToRank;
                        player.Rank = newRank;
                        Context.SaveChanges();
                        outputMessage = $"You ranked up to {newRank}. (-{pointsToRank} cheese)";
                    }
                }
                else
                {
                    var pointsNeededToRank = pointsToRank - player.Points;
                    if (newRank == Rank.Bronze)
                    {
                        outputMessage = $"You need {pointsNeededToRank} more cheese in order to prestige back to {Rank.Bronze} rank. You will lose all your cheese and upgrades, but will gain a permanent {(Int32)(Constants.PrestigeBonus * 100)}% bonus on your cheese gains.";
                    }
                    else
                    {
                        outputMessage = $"You need {pointsNeededToRank} more cheese in order to rank up to {newRank}.";
                    }
                }
            }
            else
            {
                outputMessage = $"Uh oh, you broke something. You have an invalid rank of {player.Rank}.";
            }

            TwitchClientManager.SpoolMessageAsMe(message.Channel, player, outputMessage);
        }

        public void ShowRank(ChatMessage message)
        {
            var player = GetPlayer(message);

            var nextRank = player.Rank.Next();

            String outputMessage;

            if (PointsToRank.TryGetValue(player.Rank, out Int32 pointsToRank))
            {
                String nextRankInformation;

                if (pointsToRank > player.Points)
                {

                    nextRankInformation = $"You need {pointsToRank - player.Points} more cheese to rankup to ";
                }
                else
                {
                    nextRankInformation = $"You have enough cheese ({pointsToRank}) to rankup right now to ";
                }

                if (nextRank == Rank.Bronze)
                {
                    nextRankInformation += $"prestige back to {Rank.Bronze} rank. You will lose all your cheese and upgrades, but will gain a permanent {(Int32)(Constants.PrestigeBonus * 100)}% bonus on your cheese gains.";
                }
                else
                {
                    nextRankInformation += $"{nextRank} rank.";
                }

                outputMessage = $"You are currently in {player.Rank} rank. {nextRankInformation}";
            }
            else
            {
                outputMessage = $"Uh oh, you broke something. You have an invalid rank of {player.Rank}.";
            }

            TwitchClientManager.SpoolMessageAsMe(message.Channel, player, outputMessage);

        }
    }
}
