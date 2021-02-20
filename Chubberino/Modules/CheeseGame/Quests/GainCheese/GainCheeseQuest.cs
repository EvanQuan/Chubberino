using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainCheese
{
    public abstract class GainCheeseQuest : Quest
    {
        protected GainCheeseQuest(IApplicationContext context, Random random, ITwitchClientManager client, IEmoteManager emoteManager) : base(context, random, client, emoteManager)
        {
        }
        
        protected abstract Int32 BaseRewardPoints { get; }

        protected abstract String SuccessMessage { get; }

        protected override String OnSuccess(Player player)
        {
            Int32 rewardPoints = (Int32)(BaseRewardPoints * Math.Pow((1 + (Int32)player.Rank * RewardRankMultiplier), RewardRankExponent));

            player.AddPoints(rewardPoints);
            Context.SaveChanges();

            return String.Format(SuccessMessage, rewardPoints);
        }
    }
}
