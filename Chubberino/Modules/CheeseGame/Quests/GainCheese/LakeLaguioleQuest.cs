using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainCheese
{
    public sealed class LakeLaguioleQuest : GainCheeseQuest
    {
        public LakeLaguioleQuest(IApplicationContext context, Random random, ITwitchClientManager client, IEmoteManager emoteManager) : base(context, random, client, emoteManager)
        {
        }

        protected override Int32 BaseRewardPoints => 75;

        protected override String SuccessMessage =>
            "You go fishing and catch some Taleggio Tuna. (+{0} cheese)";

        protected override String OnFailure(Player player)
        {
            return $"With the bad weather, you can't find any fish.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} travel to Lake Laguiole.";
        }
    }
}
