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
        public const String NewInfestationMessage = "A giant mouse sneaks into your factory, scaring away your workers. ";

        public const String OldInfestationMessage = "A giant mouse is still infesting your cheese factory, scaring away your workers. ";

        public const String KillOldRatMessage = "You set up a mousetrap, killing the giant mouse infesting your cheese factory. Your workers go back to the work. ";

        public const String KillNewRatMessage = "A giant mouse sneaks into your factory, but is promptly killed by a mousetrap you have set up. ";

        /// <summary>
        /// Maximum number of mice per infestation at each rank.
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
        /// Chance of mice infestation per !cheese at each rank.
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

        public HazardManager(IApplicationContext context, ITwitchClientManager client, Random random, IEmoteManager emoteManager) : base(context, client, random, emoteManager)
        {
        }

        public String UpdateMouseInfestationStatus(Player player)
        {
            String outputMessage = String.Empty;

            if (player.IsMouseInfested())
            {
                // If already mouse infested, player must deal with existing mice before infestation is over.
                // New mice cannot be added.
                Boolean isSingleMouse = player.MouseCount == 1;
                Boolean isSingleMouseTrapUsed = player.MouseTrapCount == 1;

                String mouse = isSingleMouse ? "mouse" : "mice";

                if (player.MouseTrapCount == 0)
                {
                    // Old infestation remains, uncontested
                    outputMessage = $"{player.MouseCount} giant {mouse} {(isSingleMouse ? "is" : "are")} still infesting your cheese factory, scaring away your workers. ";
                }
                else if (player.MouseTrapCount < player.MouseCount)
                {
                    // Old infestation remains, contested.
                    Int32 newMouseCount = player.MouseCount - player.MouseTrapCount;
                    outputMessage = $"You set up {(isSingleMouseTrapUsed ? "a mousetrap" : $"{player.MouseCount} mousetraps")}, killing {(isSingleMouse ? "a giant mouse" : "some of the giant mice")} infesting your cheese factory. {newMouseCount} {(newMouseCount == 1 ? "remains" : "remain")}, scaring away your workers. ";
                    player.MouseCount = newMouseCount;
                    player.MouseTrapCount = 0;
                    Context.SaveChanges();
                }
                else
                {
                    // Old infestation ends.
                    outputMessage = $"You set up {(isSingleMouseTrapUsed ? "a mousetrap" : $"{player.MouseCount} mousetraps")}, killing {(isSingleMouse ? "the giant mouse" : $"all the giant mice")} infesting your cheese factory. Your workers go back to work. ";
                    player.MouseTrapCount -= player.MouseCount;
                    player.MouseCount = 0;
                    Context.SaveChanges();
                }
            }
            else if (Random.TryPercentChance(InfestationChance[player.Rank]))
            {
                Int32 mouseCount = Random.Next(1, InfestationMaximum[player.Rank]);

                Boolean isSingleMouse = mouseCount == 1;
                String mouse = isSingleMouse ? "mouse" : "mice";

                String sneak = isSingleMouse ? "sneaks" : "sneak";

                if (player.MouseTrapCount == 0)
                {
                    // New infestation, uncontested.
                    outputMessage = $"{mouseCount} giant {mouse} {sneak} into your cheese factory, scaring away your workers. ";
                    player.MouseCount = mouseCount;
                }
                else if (mouseCount <= player.MouseTrapCount)
                {
                    // No new infestation, prevented.
                    outputMessage = $"{mouseCount} giant {mouse} {sneak} into your cheese factory, but {(isSingleMouse ? "is": "are all")} promptly killed by {(isSingleMouse ? "a mousetrap" : "mousetraps")} you have set up. ";
                    player.MouseTrapCount -= mouseCount;
                }
                else
                {
                    // New infestation, contested.
                    Int32 newMouseCount = mouseCount - player.MouseTrapCount;
                    Boolean isSingleNewMouse = newMouseCount == 1;
                    Boolean isSingleMouseTrapUsed = player.MouseTrapCount == 1;
                    outputMessage = $"{mouseCount} giant {mouse} {sneak} into your cheese factory. {newMouseCount} {(isSingleNewMouse ? "remains" : "remain")} after {player.MouseTrapCount} {(isSingleMouseTrapUsed ? "is" : "are")} killed by {(isSingleMouseTrapUsed ? "a mousetrap" : "mousetraps")} ";
                    player.MouseTrapCount = 0;
                    player.MouseCount = newMouseCount;
                }
                Context.SaveChanges();
            }

            return outputMessage;
        }
    }
}
