using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public abstract class Quest
    {
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
        /// Reward description for shop purposes. Displays base reward values.
        /// </summary>
        public String RewardDescription { get; }

        /// <summary>
        /// Rank needed to unlock.
        /// </summary>
        public Rank RankToUnlock { get; }

        /// <summary>
        /// Price to unlock.
        /// </summary>
        public Int32 Price { get; }

        public Quest(
            String location,
            String failureMessage,
            Func<Player, String, String> onSuccess,
            String rewardDescription,
            Rank rankToUnlock,
            Int32 price)
        {
            Location = location;
            FailureMessage = failureMessage;
            OnSuccess = onSuccess;
            RewardDescription = rewardDescription;
            RankToUnlock = rankToUnlock;
            Price = price;
        }
    }
}
