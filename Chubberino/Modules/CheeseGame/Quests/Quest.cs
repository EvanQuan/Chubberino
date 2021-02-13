using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public abstract class Quest : IQuest
    {
        public Quest(ApplicationContext context, Random random, IMessageSpooler spooler, IEmoteManager emoteManager)
        {
            Context = context;
            Random = random;
            Spooler = spooler;
            EmoteManager = emoteManager;
        }

        public ApplicationContext Context { get; }

        public Random Random { get; }

        public Double RewardRankMultiplier { get; set; } = 0.2;

        public IMessageSpooler Spooler { get; }

        public IEmoteManager EmoteManager { get; }

        protected Double BaseSuccessChance { get; set; } = 0.4;

        protected Double WorkerSuccessBonus { get; set; } = 0.01;

        /// <summary>
        /// Message on quest failure.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="player"></param>
        /// <returns>Formatted failure message.</returns>
        protected abstract String OnFailure(Player player);
        
        /// <summary>
        /// Message on quest success.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="player"></param>
        /// <returns>Formatted success message.</returns>
        protected abstract String OnSuccess(Player player);

        /// <summary>
        /// Message on start of quest.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        protected abstract String OnIntroduction(Player player);

        public Boolean Start(ChatMessage message, Player player)
        {
            Double successChance = BaseSuccessChance * (1 + player.WorkerCount * (WorkerSuccessBonus * ((Int32)player.LastWorkerQuestHelpUnlocked + 1)));

            Boolean successful = successChance > Random.NextDouble();

            var resultMessage = successful
                ? OnSuccess(player)
                : OnFailure(player);

            Spooler.SpoolMessage(OnIntroduction(player) + " " + resultMessage);

            return successful;
        }

        protected Boolean TryLoseWorker(Player player)
        {
            Boolean hasWorkers = player.WorkerCount > 0;

            if (hasWorkers)
            {
                player.WorkerCount--;
                Context.SaveChanges();
            }

            return hasWorkers;
        }

        protected String GetPlayerWithWorkers(Player player)
        {
            return player.WorkerCount switch
            {
                0 => "You",
                1 => "You and your worker",
                _ => $"You and your {player.WorkerCount} workers",
            };
        }
    }
}
