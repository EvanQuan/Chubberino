using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainCheese
{
    public sealed class MagnaMountainQuest : GainCheeseQuest
    {
        public MagnaMountainQuest(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculator)
            : base(context, random, client, emoteManager, calculator)
        {
        }

        protected override Int32 BaseRewardPoints => 50;

        protected override String SuccessMessage =>
            "You find a giant vein of Magna cheese and mine at it for hours.";

        protected override String OnFailure(Player player)
        {
            return $"You search the cavern depths, but with no luck.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} venture into the caves of Mount Magna.";
        }
    }
}
