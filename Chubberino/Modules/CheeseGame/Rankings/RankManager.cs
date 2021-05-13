using Chubberino.Client;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Heists;
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
            {  Rank.Bronze, 300 },
            {  Rank.Silver, 900 },
            {  Rank.Gold, 1800 },
            {  Rank.Platinum, 3600 },
            {  Rank.Diamond, 7200 },
            {  Rank.Master, 14400 },
            {  Rank.Grandmaster, 28800 },
            {  Rank.Legend, 57600 },
        };
        public IHeistManager HeistManager { get; }

        public RankManager(
            IApplicationContext context,
            ITwitchClientManager client,
            Random random,
            IEmoteManager emoteManager,
            IHeistManager heistManager)
            : base(context, client, random, emoteManager)
        {
            HeistManager = heistManager;
        }

        public void RankUp(ChatMessage message)
        {
            var player = GetPlayer(message);

            String outputMessage;

            Priority priority = Priority.Low;

            if (RanksToPoints.TryGetValue(player.Rank, out Int32 pointsToRank))
            {
                Rank nextRank = player.Rank.Next();

                if (player.Points >= pointsToRank)
                {
                    if (nextRank == Rank.None)
                    {
                        if (player.HasUnlockedAllCheeses())
                        {
                            HeistManager.LeaveAllHeists(player);
                            // Prestige instead of rank up
                            player.ResetRank();
                            player.Prestige++;
                            Context.SaveChanges();
                            outputMessage = $"You prestiged back to {Rank.Bronze} and have gained a permanent {(Int32)(Constants.PrestigeBonus * 100)}% cheese gain boost.";
                            priority = Priority.Medium;
                        }
                        else
                        {
                            outputMessage = $"You need to buy all cheese recipes in order to prestige back to {Rank.Bronze} rank. You will lose all your cheese and upgrades, but will gain a permanent {(Int32)(Constants.PrestigeBonus * 100)}% bonus on your cheese gains.";
                        }
                    }
                    else
                    {
                        player.Points -= pointsToRank;
                        player.Rank = nextRank;
                        Context.SaveChanges();
                        outputMessage = $"You ranked up to {nextRank}. {Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Negative))} (-{pointsToRank} cheese)";
                        priority = Priority.Medium;
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

            TwitchClientManager.SpoolMessageAsMe(message.Channel, player, outputMessage, priority);
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
