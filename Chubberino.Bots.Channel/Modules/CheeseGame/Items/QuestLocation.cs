using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Common.Extensions;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items;

public sealed class QuestLocation : Item
{
    private const String NoQuestsForSaleMessage = "You have already purchased every quest map.";

    public override IEnumerable<String> Names { get; } = new String[]
    {
        "Quest",
        "q",
        "quests"
    };

    public IQuestRepository QuestRepository { get; }

    public QuestLocation(IQuestRepository questRepository)
    {
        QuestRepository = questRepository;
    }

    public override Either<Int32, String> GetPrice(Player player)
        => QuestRepository.CommonQuests.TryGetNextToUnlock(player)
            .Some(quest => Either<Int32, String>.Left(quest.Price))
            .None(NoQuestsForSaleMessage);

    public override String GetSpecificNameForNotEnoughToBuy(Player player)
        => QuestRepository.CommonQuests.TryGetNextToUnlock(player)
            .Some(quest => $"a map to {quest.Location}")
            .None(NoQuestsForSaleMessage);

    public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
    {
        var temporaryPlayer = new Player()
        {
            QuestsUnlockedCount = player.QuestsUnlockedCount - quantity
        };

        List<String> questLocationsPurchased = new();

        for (Int32 i = 0; i < quantity; i++, temporaryPlayer.QuestsUnlockedCount++)
        {
            QuestRepository
                .CommonQuests
                .TryGetNextToUnlock(temporaryPlayer)
                .IfSome(quest => questLocationsPurchased.Add(quest.Location));
        }

        return $"{(quantity == 1 ? "a map" : "maps")} to {String.Join(", ", questLocationsPurchased)}";
    }

    public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        => QuestRepository
            .CommonQuests
            .TryGetNextToUnlock(player)
            .Some(nextQuestToUnlock =>
            {
                player.QuestsUnlockedCount++;

                player.Points -= nextQuestToUnlock.Price;

                return Either<Int32, String>.Left(1);
            })
            .None(NoQuestsForSaleMessage);

    public override Option<String> GetShopPrompt(Player player)
        => QuestRepository
            .CommonQuests
            .TryGetNextToUnlock(player)
            .Bind(nextQuestToUnlock =>
            {
                String questPrompt = nextQuestToUnlock.RankToUnlock > player.Rank
                    ? $"{nextQuestToUnlock.Location} ({nextQuestToUnlock.RewardDescription(player)})] unlocked at {player.Rank.Next()} rank"
                    : $"{nextQuestToUnlock.Location} ({nextQuestToUnlock.RewardDescription(player)})] for {nextQuestToUnlock.Price} cheese";

                return Option<String>.Some($"{GetBaseShopPrompt(player)} [{questPrompt}");
            });
}
