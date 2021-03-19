using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainCheese
{
    public abstract class GainCheeseQuest : Quest
    {
        protected GainCheeseQuest(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculator)
            : base(context, random, client, emoteManager, calculator)
        {
        }
        
        protected abstract Int32 BaseRewardPoints { get; }

        protected abstract String SuccessMessage { get; }

        protected override String OnSuccess(Player player)
        {
            Int32 rewardPoints = (Int32)(BaseRewardPoints * Calculator.GetQuestRewardMultiplier(player.Rank));

            player.AddPoints(rewardPoints);
            Context.SaveChanges();

            return SuccessMessage + $" (+{rewardPoints} cheese)";
        }
    }
}
