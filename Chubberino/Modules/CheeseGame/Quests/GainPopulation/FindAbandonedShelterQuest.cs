using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainPopulation
{
    public sealed class FindAbandonedShelterQuest : Quest
    {
        public FindAbandonedShelterQuest(ApplicationContext context, Random random, IMessageSpooler spooler, IEmoteManager emoteManager) : base(context, random, spooler, emoteManager)
        {
        }

        protected override String OnFailure(Player player)
        {
            return $"You quickly get lost within the woods. By nightfall, you finally find your way out.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} venture into the depths of the Fontiago Forest.";
        }

        protected override String OnSuccess(Player player)
        {
            const Int32 basePopulationReward = 2;
            Int32 rewardPopulation = (Int32)(basePopulationReward * (1 + (Int32)player.Rank * RewardRankMultiplier));
            player.PopulationCount += rewardPopulation;
            Context.SaveChanges();

            return $"You find an abandoned shelter. It takes some time to repair it, but it you finally get it in shape to house some of your workers. (+{rewardPopulation} population)";
        }
    }
}
