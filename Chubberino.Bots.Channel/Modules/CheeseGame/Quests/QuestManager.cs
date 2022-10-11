using Chubberino.Bots.Channel.Modules.CheeseGame.Emotes;
using Chubberino.Bots.Channel.Modules.CheeseGame.Hazards;
using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Common.Extensions;
using Chubberino.Common.Services;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Quests;

public sealed class QuestManager : IQuestManager
{
    public static TimeSpan QuestCooldown { get; set; } = TimeSpan.FromHours(2);
    public IApplicationContextFactory ContextFactory { get; }
    public ITwitchClientManager Client { get; }
    public Random Random { get; }
    public IEmoteManager EmoteManager { get; }
    public IDateTimeService DateTime { get; }
    public IQuestRepository QuestRepository { get; }

    public QuestManager(
        IApplicationContextFactory contextFactory,
        ITwitchClientManager client,
        Random random,
        IEmoteManager emoteManager,
        IDateTimeService dateTime,
        IQuestRepository questRepository)
    {
        ContextFactory = contextFactory;
        Client = client;
        Random = random;
        EmoteManager = emoteManager;
        DateTime = dateTime;
        QuestRepository = questRepository;
    }

    public void TryStartQuest(ChatMessage message)
    {
        using var context = ContextFactory.GetContext();

        var player = context.GetPlayer(Client, message);

        if (!player.HasQuestingUnlocked())
        {
            Client.SpoolMessageAsMe(message.Channel, player,
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
            StartQuestAtTime(message, player, now, context);
        }
        else
        {
            PromptStartQuestFailed(message, player, timeSinceLastQuestVentured);
        }
    }

    private void PromptStartQuestFailed(ChatMessage message, Player player, TimeSpan timeSinceLastQuestVentured)
    {
        var timeUntilNextQuestAvailable = QuestCooldown - timeSinceLastQuestVentured;

        var timeToWait = timeUntilNextQuestAvailable.Format();

        Client.SpoolMessageAsMe(message.Channel, player,
            $"[Quest {String.Format("{0:0.0}", player.GetQuestSuccessChance() * 100)}% success] " +
            $"You must wait {timeToWait} until you can go on your next quest. {Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Waiting))}",
            Priority.Low);
    }

    private void StartQuestAtTime(ChatMessage message, Player player, DateTime now, IApplicationContext context)
    {
        var quest = Random.NextElement(QuestRepository, player);

        StartQuest(message, player, quest);

        player.LastQuestVentured = now;

        context.SaveChanges();
    }

    private void StartQuest(ChatMessage message, Player player, Quest quest)
    {
        Double successChance = player.GetQuestSuccessChance();

        Boolean successful = Random.TryPercentChance(successChance);

        var resultMessage = successful
            ? quest.OnSuccess(player, Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Positive)))
            : quest.FailureMessage + " " + Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Negative));

        StringBuilder questPrompt = new();

        questPrompt
            .Append($"[Quest {String.Format("{0:0.0}", successChance * 100)}% success] ");

        if (quest.IsRare)
        {
            var positiveEmotes = EmoteManager.Get(message.Channel, EmoteCategory.Positive);

            questPrompt
                .Append(Random.NextElement(positiveEmotes))
                .Append(" RARE QUEST!!! ")
                .Append(Random.NextElement(positiveEmotes))
                .Append(' ');
        }

        questPrompt
            .Append($"{GetPlayerWithWorkers(player)} travel to {quest.Location}. {resultMessage}");

        Client.SpoolMessageAsMe(message.Channel, player, questPrompt.ToString());
    }

    private static String GetPlayerWithWorkers(Player player)
        => (player.IsInfested() ? 0 : player.WorkerCount) switch
        {
            0 => "You",
            1 => "You and your worker",
            _ => $"You and your workers",
        };
}
