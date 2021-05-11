using Chubberino.Client;
using Chubberino.Client.Services;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Repositories;
using Chubberino.Utility;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class QuestManager : AbstractCommandStrategy, IQuestManager
    {
        public static TimeSpan QuestCooldown { get; set; } = TimeSpan.FromHours(2);
        public IDateTimeService DateTime { get; }
        public IRepository<Quest> QuestRepository { get; }

        public QuestManager(
            IApplicationContext context,
            ITwitchClientManager client,
            Random random,
            IEmoteManager emoteManager,
            IDateTimeService dateTime,
            IRepository<Quest> questRepository)
            : base(context, client, random, emoteManager)
        {
            DateTime = dateTime;
            QuestRepository = questRepository;
        }

        public void TryStartQuest(ChatMessage message)
        {
            var player = GetPlayer(message);

            if (!player.HasQuestingUnlocked())
            {
                TwitchClientManager.SpoolMessageAsMe(message.Channel, player,
                    "[Quest 0% success] " +
                    "You are unfamiliar with the lands around you and quickly get lost. " +
                    "You must buy a quest map from the shop with \"!cheese buy quest\" before you can start questing.",
                    Priority.Low);
                return;
            }

            var now = DateTime.Now;

            var timeSinceLastQuestVentured = now - player.LastQuestVentured;

            if (timeSinceLastQuestVentured >= QuestCooldown)
            {
                player.LastQuestVentured = now;

                var quest = Random.NextElement(QuestRepository.Values, player.QuestsUnlockedCount);

                StartQuest(message, player, quest);

                player.LastQuestVentured = now;

                Context.SaveChanges();
            }
            else
            {
                var timeUntilNextQuestAvailable = QuestCooldown - timeSinceLastQuestVentured;

                var timeToWait = timeUntilNextQuestAvailable.Format();

                TwitchClientManager.SpoolMessageAsMe(message.Channel, player,
                    $"[Quest {String.Format("{0:0.0}", player.GetQuestSuccessChance() * 100)}% success] " +
                    $"You must wait {timeToWait} until you can go on your next quest. {EmoteManager.GetRandomNegativeEmote(message.Channel)}",
                    Priority.Low);
            }
        }

        private void StartQuest(ChatMessage message, Player player, Quest quest)
        {
            Double successChance = player.GetQuestSuccessChance();

            Boolean successful = Random.TryPercentChance(successChance);

            var resultMessage = successful
                ? quest.OnSuccess(player, EmoteManager.GetRandomPositiveEmote(message.Channel))
                : quest.FailureMessage + " " + EmoteManager.GetRandomNegativeEmote(message.Channel);

            Context.SaveChanges();

            TwitchClientManager.SpoolMessageAsMe(message.Channel, player,
                $"[Quest {String.Format("{0:0.0}", successChance * 100)}% success] " +
                $"{GetPlayerWithWorkers(player)} travel to {quest.Location}. {resultMessage}");
        }

        private static String GetPlayerWithWorkers(Player player)
        {
            return (player.IsInfested() ? 0 : player.WorkerCount) switch
            {
                0 => "You",
                1 => "You and your worker",
                _ => $"You and your workers",
            };
        }
    }
}
