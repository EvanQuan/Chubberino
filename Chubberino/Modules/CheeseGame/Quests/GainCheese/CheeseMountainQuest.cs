using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class CheeseMountainQuest : Quest
    {
        public CheeseMountainQuest(ApplicationContext context, Random random, IMessageSpooler spooler, IEmoteManager emoteManager) : base(context, random, spooler, emoteManager)
        {
        }

        protected override String OnFailure(Player player)
        {
            const Int32 baseRewardsPoints = 10;

            Int32 rewardPoints = (Int32)(baseRewardsPoints + (1 * RewardRankMultiplier));

            player.AddPoints(rewardPoints);
            Context.SaveChanges();

            return $"You find a few crumbs of cheese here and there, but otherwise no luck. (+{rewardPoints} cheese) {EmoteManager.GetRandomNegativeEmote()}";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} venture into a cave of Mount Magna in hopes for finding treasures hidden within.";
        }

        protected override String OnSuccess(Player player)
        {
            const Int32 baseRewardsPoints = 100;

            Int32 rewardPoints = (Int32)(baseRewardsPoints + (1 * RewardRankMultiplier));

            player.AddPoints(rewardPoints);
            Context.SaveChanges();

            return $"Score! You find a giant vein of Magna cheese and mine at it for hours. (+{rewardPoints} cheese) {EmoteManager.GetRandomPositiveEmote()}";
        }
    }
}
