using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Utility;
using System;

namespace Chubberino.Modules.CheeseGame.Hazards
{
    public sealed class HazardManager : AbstractCommandStrategy, IHazardManager
    {
        public const String NewInfestationMessage = "A giant mouse sneaks into your factory, scaring away your workers. ";

        public const String OldInfestationMessage = "A giant mouse is still infesting your cheese factory, scaring away your workers. ";

        public const String KillOldRatMessage = "You set up a mousetrap, killing the giant mouse infesting your cheese factory. Your workers go back to the work. ";

        public const String KillNewRatMessage = "A giant mouse sneaks into your factory, but is promptly killed by a mousetrap you have set up. ";

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
            else if (Random.TryPercentChance(((Double)player.Rank) / 100.0))
            {
                Int32 mouseCount = Random.Next(1, (Int32)player.Rank + 1);

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
