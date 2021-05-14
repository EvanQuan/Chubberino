using Chubberino.Client;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Utility;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Hazards
{
    public sealed class HazardManager : AbstractCommandStrategy, IHazardManager
    {
        public const String NewInfestationMessage = "A giant rat sneaks into your factory, scaring away your workers. ";

        public const String OldInfestationMessage = "A giant rat is still infesting your cheese factory, scaring away your workers. ";

        public const String KillOldRatMessage = "You set up a mousetrap, killing the giant rat infesting your cheese factory. Your workers go back to the work. ";

        public const String KillNewRatMessage = "A giant rat sneaks into your factory, but is promptly killed by a mousetrap you have set up. ";

        /// <summary>
        /// Maximum number of rats per infestation at each rank.
        /// </summary>
        public static IDictionary<Rank, Int32> InfestationMaximum { get; } = new Dictionary<Rank, Int32>()
        {
            { Rank.Bronze, 0 },
            { Rank.Silver, 3 },
            { Rank.Gold, 7 },
            { Rank.Platinum, 12 },
            { Rank.Diamond, 18 },
            { Rank.Master, 25 },
            { Rank.Grandmaster, 33 },
            { Rank.Legend, 42 },
        };

        /// <summary>
        /// Chance of rats infestation per !cheese at each rank.
        /// </summary>
        public static IDictionary<Rank, Double> InfestationChance { get; } = new Dictionary<Rank, Double>()
        {
            { Rank.Bronze, 0 },
            { Rank.Silver, 0.01 },
            { Rank.Gold, 0.012 },
            { Rank.Platinum, 0.014 },
            { Rank.Diamond, 0.016 },
            { Rank.Master, 0.018 },
            { Rank.Grandmaster, 0.02 },
            { Rank.Legend, 0.022 },
        };

        public HazardManager(
            IApplicationContext context,
            ITwitchClientManager client,
            Random random,
            IEmoteManager emoteManager)
            : base(context, client, random, emoteManager)
        {
        }

        public String UpdateInfestationStatus(Player player)
        {
            String outputMessage = String.Empty;


            if (Random.TryPercentChance(InfestationChance[player.Rank]))
            {
                Int32 ratCount = Random.Next(1, InfestationMaximum[player.Rank]);

                Boolean isSingle = ratCount == 1;
                String rat = isSingle ? "rat" : "rats";

                String sneak = isSingle ? "sneaks" : "sneak";

                // New rat count after new infestation and removed from mousetraps.
                Int32 newRatCount = 0.Max(ratCount + player.MouseCount - player.MouseTrapCount);

                if (player.MouseTrapCount == 0)
                {
                    // New infestation, uncontested.
                    outputMessage = $"{ratCount} giant {rat} {sneak} into your cheese factory, scaring away your workers. ";
                    player.MouseCount += ratCount;
                }
                else if (newRatCount <= 0)
                {
                    // New infestation (and maybe old infestation) eliminated.
                    outputMessage = $"{ratCount} giant {rat} {sneak} into your cheese factory, but {(isSingle ? "is": "are all")} promptly killed by {(isSingle ? "a mousetrap" : "mousetraps")} you have set up. ";
                    player.MouseTrapCount -= (ratCount + player.MouseCount);
                    player.MouseCount = 0;
                }
                else
                {
                    // New infestation, contested.
                    Boolean isSingleNewRat = newRatCount == 1;
                    Boolean isSingleMouseTrapUsed = player.MouseTrapCount == 1;
                    outputMessage = $"{ratCount} giant {rat} {sneak} into your cheese factory. {newRatCount} {(isSingleNewRat ? "remains" : "remain")} after {player.MouseTrapCount} {(isSingleMouseTrapUsed ? "is" : "are")} killed by {(isSingleMouseTrapUsed ? "a mousetrap" : "mousetraps")} ";
                    player.MouseCount = newRatCount;
                    player.MouseTrapCount = 0;
                }
                Context.SaveChanges();
            }
            else if (player.IsInfested())
            {
                // Player is already infested, but not new rats were added.
                Boolean isSingle = player.MouseCount == 1;
                Boolean isSingleMouseTrapUsed = player.MouseTrapCount == 1;

                String rat = isSingle ? "rat" : "rats";

                if (player.MouseTrapCount == 0)
                {
                    // Old infestation remains, uncontested
                    outputMessage = $"{player.MouseCount} giant {rat} {(isSingle ? "is" : "are")} still infesting your cheese factory, scaring away your workers. ";
                }
                else if (player.MouseTrapCount < player.MouseCount)
                {
                    // Old infestation remains, contested.
                    Int32 newRatCount = player.MouseCount - player.MouseTrapCount;
                    outputMessage = $"You set up {(isSingleMouseTrapUsed ? "a mousetrap" : $"{player.MouseCount} mousetraps")}, killing {(isSingle ? "a giant rat" : "some of the giant rats")} infesting your cheese factory. {newRatCount} {(newRatCount == 1 ? "remains" : "remain")}, scaring away your workers. ";
                    player.MouseCount = newRatCount;
                    player.MouseTrapCount = 0;
                    Context.SaveChanges();
                }
                else
                {
                    // Old infestation ends.
                    outputMessage = $"You set up {(isSingleMouseTrapUsed ? "a mousetrap" : $"{player.MouseCount} mousetraps")}, killing {(isSingle ? "the giant rat" : $"all the giant rats")} infesting your cheese factory. Your workers go back to work. ";
                    player.MouseTrapCount -= player.MouseCount;
                    player.MouseCount = 0;
                    Context.SaveChanges();
                }
            }

            return outputMessage;
        }
    }
}
