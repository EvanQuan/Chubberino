using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public abstract class Quest : IQuest
    {
        public Quest(IApplicationContext context, Random random, IMessageSpooler spooler, IEmoteManager emoteManager)
        {
            Context = context;
            Random = random;
            Spooler = spooler;
            EmoteManager = emoteManager;
        }

        public IApplicationContext Context { get; }

        public Random Random { get; }

        public Double RewardRankMultiplier { get; set; } = 1;

        public Double RewardRankExponent { get; set; } = 1.7;

        public IMessageSpooler Spooler { get; }

        public IEmoteManager EmoteManager { get; }

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
            Double successChance = player.GetQuestSuccessChance();

            Boolean successful = successChance > Random.NextDouble();

            Boolean useChannelEmotes = message.Channel.Equals("ChubbehMouse", StringComparison.OrdinalIgnoreCase);

            var resultMessage = successful
                ? OnSuccess(player) + " " + EmoteManager.GetRandomPositiveEmote(useChannelEmotes)
                : OnFailure(player) + " " + EmoteManager.GetRandomNegativeEmote(useChannelEmotes);

            Spooler.SpoolMessage(message.Channel, $"{player.GetDisplayName()} [{Math.Round((successChance * 100), 2)}% success] {OnIntroduction(player)} {resultMessage}");

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
