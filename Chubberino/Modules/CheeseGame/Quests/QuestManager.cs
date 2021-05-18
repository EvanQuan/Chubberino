using Chubberino.Client;
using Chubberino.Client.Services;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Utility;
using System;
using System.Text;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class QuestManager : AbstractCommandStrategy, IQuestManager
    {
        public static TimeSpan QuestCooldown { get; set; } = TimeSpan.FromHours(2);
        public IDateTimeService DateTime { get; }
        public IQuestRepository QuestRepository { get; }

        public QuestManager(
            IApplicationContext context,
            ITwitchClientManager client,
            Random random,
            IEmoteManager emoteManager,
            IDateTimeService dateTime,
            IQuestRepository questRepository)
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
                var quest = Random.NextElement(QuestRepository, player.QuestsUnlockedCount - 1);

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
                    $"You must wait {timeToWait} until you can go on your next quest. {Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Waiting))}",
                    Priority.Low);
            }
        }

        private void StartQuest(ChatMessage message, Player player, Quest quest)
        {
            Double successChance = player.GetQuestSuccessChance();

            Boolean successful = Random.TryPercentChance(successChance);

            var resultMessage = successful
                ? quest.OnSuccess(player, Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Positive)))
                : quest.FailureMessage + " " + Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Negative));

            Context.SaveChanges();

            StringBuilder questPrompt = new();

            questPrompt
                .Append($"[Quest {String.Format("{0:0.0}", successChance * 100)}% success] ");

            if (quest.IsRare)
            {
                var positiveEmotes = EmoteManager.Get(message.Channel, EmoteCategory.Positive);

                questPrompt
                    .Append(Random.NextElement(positiveEmotes))
                    .Append(" RARE QUEST !!! ")
                    .Append(Random.NextElement(positiveEmotes))
                    .Append(' ');
            }

            questPrompt
                .Append($"{GetPlayerWithWorkers(player)} travel to {quest.Location}. {resultMessage}");

            TwitchClientManager.SpoolMessageAsMe(message.Channel, player, questPrompt.ToString());
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
