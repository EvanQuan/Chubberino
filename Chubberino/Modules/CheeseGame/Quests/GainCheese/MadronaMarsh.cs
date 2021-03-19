using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainCheese
{
    public sealed class MadronaMarsh : GainCheeseQuest
    {
        public MadronaMarsh(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculator)
            : base(context, random, client, emoteManager, calculator)
        {
        }

        protected override Int32 BaseRewardPoints => 120;

        protected override String SuccessMessage =>
            "You find some cheese hidden in the depths that must have been aging for decades.";

        protected override String OnFailure(Player player)
        {
            return $"You get lost in the fog, and with some trouble, return safely.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} travel to the Madrona Marsh.";
        }
    }
}
