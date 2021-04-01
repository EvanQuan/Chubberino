using Chubberino.Client;
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
        public static IReadOnlyDictionary<Rank, Int32> RanksToPoints { get; } = new Dictionary<Rank, Int32>()
        {
            {  Rank.Bronze, 250 },
            {  Rank.Silver, 500 },
            {  Rank.Gold, 1000 },
            {  Rank.Platinum, 2000 },
            {  Rank.Diamond, 4000 },
            {  Rank.Master, 8000 },
            {  Rank.Grandmaster, 16000 },
            {  Rank.Legend, 32000 },
        };

        public RankManager(IApplicationContext context, ITwitchClientManager client, Random random, IEmoteManager emoteManager)
            : base(context, client, random, emoteManager)
        {
        }

        public void RankUp(ChatMessage message)
        {
            var player = GetPlayer(message);

            String outputMessage;

            if (RanksToPoints.TryGetValue(player.Rank, out Int32 pointsToRank))
            {
                Rank nextRank = player.Rank.Next();

                if (player.Points >= pointsToRank)
                {
                    if (nextRank == Rank.None)
                    {
                        // Prestige instead of rank up
                        player.ResetRank();
                        player.Prestige++;
                        Context.SaveChanges();
                        outputMessage = $"You prestiged back to {Rank.Bronze} and have gained a permanent {(Int32)(Constants.PrestigeBonus * 100)}% cheese gain boost.";
                    }
                    else
                    {
                        player.Points -= pointsToRank;
                        player.Rank = nextRank;
                        Context.SaveChanges();
                        outputMessage = $"You ranked up to {nextRank}. {EmoteManager.GetRandomPositiveEmote(message.Channel)} (-{pointsToRank} cheese)";
                    }
                }
                else
                {
                    var pointsNeededToRank = pointsToRank - player.Points;
                    if (nextRank == Rank.None)
                    {
                        outputMessage = $"You need {pointsNeededToRank} more cheese in order to prestige back to {Rank.Bronze} rank. You will lose all your cheese and upgrades, but will gain a permanent {(Int32)(Constants.PrestigeBonus * 100)}% bonus on your cheese gains.";
                    }
                    else
                    {
                        outputMessage = $"You need {pointsNeededToRank} more cheese in order to rank up to {nextRank}.";
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

            if (RanksToPoints.TryGetValue(player.Rank, out Int32 pointsToRank))
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

                if (nextRank == Rank.None)
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

            TwitchClientManager.SpoolMessageAsMe(message.Channel, player, outputMessage, Priority.Low);

        }
    }
}
