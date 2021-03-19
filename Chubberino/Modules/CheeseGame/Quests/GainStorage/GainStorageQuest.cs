using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainStorage
{
    public abstract class GainStorageQuest : Quest
    {
        protected abstract Int32 BaseRewardStorage { get; }

        protected abstract String SuccessMessage { get; }

        protected GainStorageQuest(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculation)
            : base(context, random, client, emoteManager, calculation)
        {
        }

        protected override String OnSuccess(Player player)
        {
            Int32 rewardStorageWithMultiplier = (Int32)(BaseRewardStorage * player.GetStorageUpgradeMultiplier());

            // We only add the base storage value in the database, but display the multiplied value to the user.
            player.MaximumPointStorage += BaseRewardStorage;
            Context.SaveChanges();

            return SuccessMessage + $" (+{rewardStorageWithMultiplier} storage)";
        }
    }
}
