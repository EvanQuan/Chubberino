using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Ranks;
using System;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public abstract class Quest
    {
        /// <summary>
        /// Base quest success chance.
        /// </summary>
        public const Double BaseSuccessChance = 0.25;

        /// <summary>
        /// Name of the quest location.
        /// </summary>
        public String Location { get; }

        /// <summary>
        /// Message on failure.
        /// </summary>
        public String FailureMessage { get; }

        /// <summary>
        /// Modifies player on success.
        /// Returns success message.
        /// </summary>
        public Func<Player, String, String> OnSuccess { get; }

        /// <summary>
        /// Reward description for shop purposes. Displays reward values after player multipliers.
        /// </summary>
        public Func<Player, String> RewardDescription { get; }

        /// <summary>
        /// Rank needed to unlock.
        /// </summary>
        public Rank RankToUnlock { get; }

        /// <summary>
        /// Price to unlock.
        /// </summary>
        public Int32 Price { get; }

        /// <summary>
        /// Indicates this is a rare quest.
        /// </summary>
        public Boolean IsRare { get; }

        public Quest(
            String location,
            String failureMessage,
            Func<Player, String, String> onSuccess,
            Func<Player, String> rewardDescription,
            Rank rankToUnlock,
            Double rankPricePercentPrice,
            Boolean isRare = false)
        {
            Location = location;
            FailureMessage = failureMessage;
            OnSuccess = onSuccess;
            RewardDescription = rewardDescription;
            RankToUnlock = rankToUnlock;
            Price = (Int32) (RankManager.RanksToPoints[RankToUnlock] * rankPricePercentPrice);
            IsRare = isRare;
        }
    }
}
