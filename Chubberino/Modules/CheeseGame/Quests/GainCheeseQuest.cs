using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class GainCheeseQuest : Quest
    {
        public GainCheeseQuest(
            String location,
            String failureMessage,
            String successMessage,
            Int32 rewardPoints,
            Rank rankToUnlock,
            Double rankPricePercentPrice)
            : base(location,
                  failureMessage,
                  (player, emote) =>
                  {
                      Int32 finalPoints = (Int32)(rewardPoints * player.NextQuestRewardUpgradeUnlock.GetQuestRewardMultiplier());
                      player.AddPoints(finalPoints);
                      return $"{successMessage} {emote} (+{finalPoints} cheese)";
                  },
                  player => $"+{(Int32)(rewardPoints * player.NextQuestRewardUpgradeUnlock.GetQuestRewardMultiplier())} cheese",
                  rankToUnlock,
                  rankPricePercentPrice)
        {
        }
    }
}
