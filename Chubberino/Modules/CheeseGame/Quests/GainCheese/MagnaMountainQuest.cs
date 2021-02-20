using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainCheese
{
    public sealed class MagnaMountainQuest : GainCheeseQuest
    {
        public MagnaMountainQuest(IApplicationContext context, Random random, ITwitchClientManager client, IEmoteManager emoteManager) : base(context, random, client, emoteManager)
        {
        }

        protected override Int32 BaseRewardPoints => 100;

        protected override String SuccessMessage =>
            "You find a giant vein of Magna cheese and mine at it for hours. (+{0} cheese)";

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
