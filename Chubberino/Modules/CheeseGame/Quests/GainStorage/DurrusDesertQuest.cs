using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainStorage
{
    public sealed class DurrusDesertQuest : GainStorageQuest
    {
        public DurrusDesertQuest(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculator)
            : base(context, random, client, emoteManager, calculator)
        {
        }

        protected override Int32 BaseRewardStorage => Constants.ShopStorageQuantity;

        protected override String SuccessMessage { get; }
            = "You come across an ancient tomb. It appears to be empty inside, so you claim it for yourself to store cheese.";

        protected override String OnFailure(Player player)
        {
            return "The dunes go on forever, and you quickly go back without success.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} travel into the Durrus Desert.";
        }
    }
}
