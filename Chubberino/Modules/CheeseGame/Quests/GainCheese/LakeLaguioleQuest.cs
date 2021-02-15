using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainCheese
{
    public sealed class LakeLaguioleQuest : GainCheeseQuest
    {
        public LakeLaguioleQuest(ApplicationContext context, Random random, IMessageSpooler spooler, IEmoteManager emoteManager) : base(context, random, spooler, emoteManager)
        {
        }

        protected override Int32 BaseRewardPoints => 50;

        protected override String SuccessMessage =>
            "You fish for some Taleggio Tuna and find a catch quite a few. (+{0} cheese)";

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
