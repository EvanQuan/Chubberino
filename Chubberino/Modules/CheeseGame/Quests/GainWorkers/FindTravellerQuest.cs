using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainWorkers
{
    public sealed class FindTravellerQuest : Quest
    {
        public FindTravellerQuest(ApplicationContext context, Random random, IMessageSpooler spooler, IEmoteManager emoteManager) : base(context, random, spooler, emoteManager)
        {
        }

        protected override String OnFailure(Player player)
        {
            return $"You search around and don't find anything interesting. You return to your cheese factory empty-handed.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} venture out onto the roads of the Panela Plains.";
        }

        protected override String OnSuccess(Player player)
        {
            player.WorkerCount++;
            Context.SaveChanges();

            return "You come across a lonely traveller looking for work. He decides to join your efforts. (+1 worker)";
        }
    }
}
