using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Ranks;
using System;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class GainStorageQuest : Quest
    {
        public GainStorageQuest(
            String location,
            String failureMessage,
            String successMessage,
            Int32 rewardStorage)
            : base(location, failureMessage,
                  (player, emote) =>
                  {
                      Int32 rewardStorageWithMultiplier = (Int32)(rewardStorage * player.GetStorageUpgradeMultiplier());

                      // We only add the base storage value in the database, but display the multiplied value to the user.
                      player.MaximumPointStorage += rewardStorage;

                      return $"{successMessage} {emote} (+{rewardStorageWithMultiplier} storage)";
                  },
                  player => $"+{(Int32)(rewardStorage * player.GetStorageUpgradeMultiplier())} storage",
                  Rank.Bronze,
                  0,
                  isRare: true)
        {
        }
    }
}
