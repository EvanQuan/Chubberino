using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
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

        public RankManager(ApplicationContext context, IMessageSpooler spooler) : base(context, spooler)
        {
        }

        public void RankUp(ChatMessage message)
        {
            var player = GetPlayer(message);

            if (PointsToRank.TryGetValue(player.Rank, out Int32 pointsToRank))
            {
                Rank newRank = player.Rank.Next();

                if (player.Points >= pointsToRank)
                {
                    player.Points -= pointsToRank;
                    player.Rank = newRank;
                    Context.SaveChanges();
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You ranked up to {newRank} (-{pointsToRank} cheese).");
                }
                else
                {
                    var pointsNeededToRank = pointsToRank - player.Points;
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} You need {pointsNeededToRank} more cheese in order to rank up to {newRank}.");
                }
            }
            else
            {
                Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)} Uh oh, you broke something. You have an invalid rank of {player.Rank}.");
            }
        }
    }
}
