using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainCheese
{
    public sealed class CaciottaCliff : GainCheeseQuest
    {
        public CaciottaCliff(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculator)
            : base(context, random, client, emoteManager, calculator)
        {
        }

        protected override Int32 BaseRewardPoints => 90;

        protected override String SuccessMessage =>
            "You find some cheese along the edge of the cliffside, which you carefully take.";

        protected override String OnFailure(Player player)
        {
            return "The heights get to you, and you go back without finding anything.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} travel to the Caciotta Cliff.";
        }
    }
}
