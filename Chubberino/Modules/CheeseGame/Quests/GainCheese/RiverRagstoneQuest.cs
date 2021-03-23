using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainCheese
{
    public sealed class RiverRagstoneQuest : GainCheeseQuest
    {
        public RiverRagstoneQuest(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculator)
            : base(context, random, client, emoteManager, calculator)
        {
        }

        protected override Int32 BaseRewardPoints => 60;

        protected override String SuccessMessage =>
            "You find some cheese floating down the stream and grab it before it gets away.";

        protected override String OnFailure(Player player)
        {
            return "You sit at the river for hours without anything appearing.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} travel to the River Ragstone.";
        }
    }
}
