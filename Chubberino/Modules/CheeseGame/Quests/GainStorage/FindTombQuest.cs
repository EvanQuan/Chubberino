using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainStorage
{
    public sealed class FindTombQuest : Quest
    {
        public FindTombQuest(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculator)
            : base(context, random, client, emoteManager, calculator)
        {
        }

        protected override String OnFailure(Player player)
        {
            return $"The dunes go on forever, and you quickly go back without success.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} travel into the Durrus Desert.";
        }

        protected override String OnSuccess(Player player)
        {
            const Int32 baseRewardStorage = 20;

            Int32 rewardStorage = (Int32)(baseRewardStorage * Calculator.GetQuestRewardMultiplier(player.Rank));

            // We only add the base storage value in the database, but display the multiplied value to the user.
            Int32 rewardStorageWithMultiplier = (Int32)(rewardStorage * player.GetStorageUpgradeMultiplier());

            player.MaximumPointStorage += rewardStorage;
            Context.SaveChanges();

            return $"You come across an ancient tomb. It appears to be empty inside, so you claim it for yourself to store cheese. (+{rewardStorageWithMultiplier} storage)";
        }
    }
}
