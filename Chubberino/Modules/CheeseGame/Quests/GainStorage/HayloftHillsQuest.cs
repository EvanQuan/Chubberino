using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainStorage
{
    public sealed class HayloftHillsQuest : GainStorageQuest
    {
        public HayloftHillsQuest(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculation)
            : base(context, random, client, emoteManager, calculation)
        {
        }

        protected override Int32 BaseRewardStorage => (Int32)(Constants.ShopStorageQuantity * 0.5);

        protected override String SuccessMessage { get; }
        = "An abandoned hut sits atop a hill, which you claim to store cheese.";

        protected override String OnFailure(Player player)
        {
            return "You quickly tire and turn back.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} travel into the Hayloft Hills.";
        }
    }
}
