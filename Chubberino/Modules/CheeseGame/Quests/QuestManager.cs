using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Quests
{
    class QuestManager : AbstractCommandStrategy, IQuestManager
    {
        private IList<IQuest> Quests { get; }
        public static TimeSpan QuestCooldown { get; set; } = TimeSpan.FromHours(2);

        public QuestManager(IApplicationContext context, ITwitchClientManager client, Random random, IEmoteManager emoteManager)
            : base(context, client, random, emoteManager)
        {
            Quests = new List<IQuest>();
        }

        public IQuestManager AddQuest(IQuest quest)
        {
            Quests.Add(quest);

            return this;
        }

        public void StartQuest(ChatMessage message)
        {
            var player = GetPlayer(message);
            var now = DateTime.Now;

            var timeSinceLastQuestVentured = now - player.LastQuestVentured;

            if (timeSinceLastQuestVentured >= QuestCooldown)
            {
                var quest = Quests[Random.Next(Quests.Count)];

                quest.Start(message, player);

                player.LastQuestVentured = now;

                Context.SaveChanges();
            }
            else
            {
                var timeUntilNextQuestAvailable = QuestCooldown - timeSinceLastQuestVentured;

                var timeToWait = Format(timeUntilNextQuestAvailable);

                TwitchClientManager.Client.SpoolMessage(message.Channel, $"{player.GetDisplayName()}, you must wait {timeToWait} until you can go on your next quest.");
            }
        }
    }
}
