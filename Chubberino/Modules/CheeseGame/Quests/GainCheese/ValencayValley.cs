using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainCheese
{
    public sealed class ValencayValley : GainCheeseQuest
    {
        public ValencayValley(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculation)
            : base(context, random, client, emoteManager, calculation)
        {
        }

        protected override Int32 BaseRewardPoints => 70;

        protected override String SuccessMessage { get; }
        = "You find a small cave in the side of the valley, containing a treasure trove of cheese.";

        protected override String OnFailure(Player player)
        {
            return "The valley winds on forever, and you return without anything.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} travel into the Valencay Valley.";
        }
    }
}
