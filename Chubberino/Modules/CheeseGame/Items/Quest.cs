using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Repositories;
using Chubberino.Utility;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Items
{
    public sealed class Quest : Item
    {
        public override IEnumerable<String> Names { get; } = new String[] { "Quest", "q", "quests" };

        public IRepository<Quests.Quest> QuestRepository { get; }

        public Quest(IRepository<Quests.Quest> questRepository)
        {
            QuestRepository = questRepository;
        }

        public override Int32 GetPrice(Player player)
        {
            if (QuestRepository.TryGetNextToUnlock(player, out var quest))
            {
                return quest.Price;
            }

            return Int32.MaxValue;
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            if (QuestRepository.TryGetNextToUnlock(player, out var quest))
            {
                return $"a map to {quest.Location}";
            }

            return UnexpectedErrorMessage;
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            var temporaryPlayer = new Player()
            {
                QuestsUnlockedCount = player.QuestsUnlockedCount - quantity
            };

            List<String> questLocationsPurchased = new();

            for (Int32 i = 0; i < quantity; i++, temporaryPlayer.QuestsUnlockedCount++)
            {
                QuestRepository.TryGetNextToUnlock(temporaryPlayer, out var quest);

                questLocationsPurchased.Add(quest.Location);
            }

            return $"{(quantity == 1 ? "a map" : "maps")} to {String.Join(", ", questLocationsPurchased)}";
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            if (!QuestRepository.TryGetNextToUnlock(player, out var nextQuestToUnlock))
            {
                return () => UnexpectedErrorMessage;
            }

            player.QuestsUnlockedCount++;

            player.Points -= nextQuestToUnlock.Price;

            return () => 1;
        }

        public override String GetShopPrompt(Player player)
        {
            String questPrompt;
            if (QuestRepository.TryGetNextToUnlock(player, out Quests.Quest nextQuestToUnlock))
            {
                if (nextQuestToUnlock.RankToUnlock > player.Rank)
                {
                    questPrompt = $"{nextQuestToUnlock.Location} ({nextQuestToUnlock.RewardDescription(player)})] unlocked at {player.Rank.Next()} rank"; 
                }
                else
                {
                    questPrompt = $"{nextQuestToUnlock.Location} ({nextQuestToUnlock.RewardDescription(player)})] for {nextQuestToUnlock.Price} cheese"; 
                }
            }
            else
            {
                questPrompt = "OUT OF ORDER]";
            }

            return $"{base.GetShopPrompt(player)} [{questPrompt}";
        }
    }
}
