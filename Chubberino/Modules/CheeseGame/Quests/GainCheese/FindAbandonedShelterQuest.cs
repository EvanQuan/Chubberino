using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Quests.GainCheese;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainPopulation
{
    public sealed class FindAbandonedShelterQuest : GainCheeseQuest
    {
        public FindAbandonedShelterQuest(ApplicationContext context, Random random, IMessageSpooler spooler, IEmoteManager emoteManager) : base(context, random, spooler, emoteManager)
        {
        }

        protected override String OnFailure(Player player)
        {
            return $"You quickly get lost within the woods. By nightfall, you finally find your way out empty-handed.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} venture into the depths of the Fontiago Forest.";
        }

        protected override Int32 BaseRewardPoints => 75;

        protected override String SuccessMessage =>
            "You find a hidden cache. Inside is an impressive assortment of cheeses. (+{0} cheese)";
    }
}
