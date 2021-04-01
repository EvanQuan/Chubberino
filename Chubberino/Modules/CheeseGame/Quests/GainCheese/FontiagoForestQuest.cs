using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainCheese
{
    public sealed class FontiagoForestQuest : GainCheeseQuest
    {
        public FontiagoForestQuest(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculator)
            : base(context, random, client, emoteManager, calculator)
        {
        }

        protected override Int32 BaseRewardPoints => 25;

        protected override String SuccessMessage =>
            "You find a hidden cache. Inside is an impressive assortment of cheeses.";

        protected override String OnFailure(Player player)
        {
            return $"You quickly get lost within the woods. By nightfall, you finally find your way out empty-handed.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} venture into the depths of the Fontiago Forest.";
        }
    }
}
