using Chubberino.Bots.Channel.Modules.CheeseGame.Items;
using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Database.Models;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items;

public sealed class When_getting_the_next_quest_to_unlock
{
    private static QuestLocation QuestLocation { get; }

    private static IQuestRepository Repository { get; }

    static When_getting_the_next_quest_to_unlock()
    {
        Repository = new QuestRepository();
        QuestLocation = new QuestLocation(Repository);
    }

    public sealed class Given_a_new_player
    {
        [Fact]
        public void Then_first_quest_is_returned()
        {
            var player = new Player();
            var firstQuest = Repository.CommonQuests[0].Location;

            var result = QuestLocation.GetShopPrompt(player);

            result.IsSome.Should().BeTrue();
            result.IfSome(prompt =>
                prompt.Should().Contain(firstQuest));
        }
    }

    public sealed class Given_all_quests_already_unlocked
    {
        [Fact]
        public void Then_no_quest_is_returned()
        {
            var player = new Player
            {
                QuestsUnlockedCount = Repository.CommonQuests.Count
            };

            var prompt = QuestLocation.GetShopPrompt(player);

            prompt.IsNone.Should().BeTrue();
        }
    }
}
