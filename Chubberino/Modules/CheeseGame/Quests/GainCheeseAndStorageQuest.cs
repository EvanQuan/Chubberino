using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class GainCheeseAndStorageQuest : Quest
    {
        public GainCheeseAndStorageQuest(
            String location,
            String failureMessage,
            String successMessage,
            Int32 rewardPoints,
            Int32 rewardStorage,
            Rank rankToUnlock,
            Double rankPricePercentPrice)
            : base(location,
                  failureMessage,
                  (player, emote) =>
                  {
                      Int32 finalPoints = player.GetModifiedPoints(rewardPoints);
                      player.AddPoints(finalPoints);

                      Int32 rewardStorageWithMultiplier = (Int32)(rewardStorage * player.GetStorageUpgradeMultiplier());

                      // We only add the base storage value in the database, but display the multiplied value to the user.
                      player.MaximumPointStorage += rewardStorage;

                      return $"{successMessage} {emote} (+{finalPoints} cheese, +{rewardStorageWithMultiplier} storage)";
                  },
                  player => $"+{player.GetModifiedPoints(rewardPoints)} cheese, +{(Int32)(rewardStorage * player.GetStorageUpgradeMultiplier())} storage",
                  rankToUnlock,
                  rankPricePercentPrice)
        {
        }
    }
}
