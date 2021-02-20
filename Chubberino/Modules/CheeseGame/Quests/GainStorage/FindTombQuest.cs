using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainStorage
{
    public sealed class FindTombQuest : Quest
    {
        public FindTombQuest(IApplicationContext context, Random random, IMessageSpooler spooler, IEmoteManager emoteManager) : base(context, random, spooler, emoteManager)
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
            const Int32 baseRewardStorage = 50;

            Int32 rewardStorage = (Int32)(baseRewardStorage * (1 + (Int32)player.Rank * RewardRankMultiplier));

            player.MaximumPointStorage += rewardStorage;
            Context.SaveChanges();

            return $"Score! You come across an ancient tomb. There does not appear to be anything inside, so you claim it for yourself to store cheese. (+{rewardStorage} storage)";
        }
    }
}
