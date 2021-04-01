using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainCheese
{
    public sealed class WeichkaeseWoods : GainCheeseQuest
    {
        public WeichkaeseWoods(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculator)
            : base(context, random, client, emoteManager, calculator)
        {
        }

        protected override Int32 BaseRewardPoints => 40;

        protected override String SuccessMessage =>
            "You find a haunted mansion secluded in the maze of trees. Inside is some strange floating cheese, which you take.";

        protected override String OnFailure(Player player)
        {
            return "You get scared by the spooky noises, and you turn back.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} travel to the Weichkaese Woods.";
        }
    }
}
