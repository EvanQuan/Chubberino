using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Ranks;
using Chubberino.Utility;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Hazards
{
    public sealed class HazardManager : IHazardManager
    {
        public static class InfestationMaximum
        {
            public const Int32 Bronze = 0;
            public const Int32 Silver = 3;
            public const Int32 Gold = 7;
            public const Int32 Platinum = 12;
            public const Int32 Diamond = 18;
            public const Int32 Master = 25;
            public const Int32 Grandmaster = 33;
            public const Int32 Legend = 42;
        }

        /// <summary>
        /// Maximum number of rats per infestation at each rank.
        /// </summary>
        public static IDictionary<Rank, Int32> InfestationMaximums { get; } = new Dictionary<Rank, Int32>()
        {
            { Rank.Bronze, InfestationMaximum.Bronze },
            { Rank.Silver, InfestationMaximum.Silver },
            { Rank.Gold, InfestationMaximum.Gold },
            { Rank.Platinum, InfestationMaximum.Platinum },
            { Rank.Diamond, InfestationMaximum.Diamond },
            { Rank.Master, InfestationMaximum.Master },
            { Rank.Grandmaster, InfestationMaximum.Grandmaster },
            { Rank.Legend, InfestationMaximum.Legend },
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

        public Random Random { get; }

        public HazardManager(Random random)
        {
            Random = random;
        }

        public String UpdateInfestationStatus(Player player)
        {
            String outputMessage = String.Empty;

            if (Random.TryPercentChance(InfestationChance[player.Rank]))
            {
                Int32 ratCount = Random.Next(1, InfestationMaximums[player.Rank] + 1);

                Boolean isSingle = ratCount == 1;
                String rat = isSingle ? "rat" : "rats";

                String sneak = isSingle ? "sneaks" : "sneak";

                // New rat count after new infestation and removed from mousetraps.
                Int32 newRatCount = 0.Max(ratCount + player.RatCount - player.MouseTrapCount);

                if (player.MouseTrapCount == 0)
                {
                    // New infestation, uncontested.
                    outputMessage = $"{ratCount} giant {rat} {sneak} into your cheese factory, scaring away your workers. ";
                    player.RatCount += ratCount;
                }
                else if (newRatCount <= 0)
                {
                    // New infestation (and maybe old infestation) eliminated.
                    outputMessage = $"{ratCount} giant {rat} {sneak} into your cheese factory, but {(isSingle ? "is": "are all")} promptly killed by {(isSingle ? "a mousetrap" : "mousetraps")} you have set up. ";
                    player.MouseTrapCount -= (ratCount + player.RatCount);
                    player.RatCount = 0;
                }
                else
                {
                    // New infestation, contested.
                    Boolean isSingleNewRat = newRatCount == 1;
                    Boolean isSingleMouseTrapUsed = player.MouseTrapCount == 1;
                    outputMessage = $"{ratCount} giant {rat} {sneak} into your cheese factory. " +
                        $"{newRatCount} {(isSingleNewRat ? "remains" : "remain")} after {player.MouseTrapCount} {(isSingleMouseTrapUsed ? "is" : "are")} killed by {(isSingleMouseTrapUsed ? "a mousetrap" : "mousetraps")} ";
                    player.RatCount = newRatCount;
                    player.MouseTrapCount = 0;
                }
            }
            else if (player.IsInfested())
            {
                // Player is already infested, but not new rats were added.
                Boolean isSingle = player.RatCount == 1;
                Boolean isSingleMouseTrapUsed = player.MouseTrapCount == 1;

                String rat = isSingle ? "rat" : "rats";

                if (player.MouseTrapCount == 0)
                {
                    // Old infestation remains, uncontested
                    outputMessage = $"{player.RatCount} giant {rat} {(isSingle ? "is" : "are")} still infesting your cheese factory, scaring away your workers. ";
                }
                else if (player.MouseTrapCount < player.RatCount)
                {
                    // Old infestation remains, contested.
                    Int32 newRatCount = player.RatCount - player.MouseTrapCount;
                    outputMessage = $"You set up {(isSingleMouseTrapUsed ? "a mousetrap" : $"{player.RatCount} mousetraps")}, killing {(isSingle ? "a giant rat" : "some of the giant rats")} infesting your cheese factory. " +
                        $"{newRatCount} {(newRatCount == 1 ? "remains" : "remain")}, scaring away your workers. ";
                    player.RatCount = newRatCount;
                    player.MouseTrapCount = 0;
                }
                else
                {
                    // Old infestation ends.
                    outputMessage = $"You set up {(isSingleMouseTrapUsed ? "a mousetrap" : $"{player.RatCount} mousetraps")}, killing {(isSingle ? "the giant rat" : $"all the giant rats")} infesting your cheese factory. " +
                        $"Your workers go back to work. ";
                    player.MouseTrapCount -= player.RatCount;
                    player.RatCount = 0;
                }
            }

            return outputMessage;
        }
    }
}
