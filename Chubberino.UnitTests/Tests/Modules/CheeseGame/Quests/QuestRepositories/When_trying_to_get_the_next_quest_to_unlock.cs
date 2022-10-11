using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Database.Models;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Quests.QuestRepositories;

public sealed class When_trying_to_get_the_next_quest_to_unlock
{
    public static IReadOnlyList<Quest> Quests { get; } = new QuestRepository().CommonQuests;

    public sealed class Given_a_new_player
    {
        [Fact]
        public void Then_first_quest_is_returned()
        {
            var player = new Player();
            var firstQuest = Quests[0];

            var result = Quests.TryGetNextToUnlock(player);

            result.IsSome.Should().BeTrue();
            result.IfSome(quest =>
                quest.Should().Be(firstQuest));
        }
    }

    public sealed class Given_next_quest_is_the_last
    {
        [Fact]
        public void Then_last_quest_is_returned()
        {
            var player = new Player
            {
                QuestsUnlockedCount = Quests.Count - 1
            };
            var lastQuest = Quests[Quests.Count - 1];

            var result = Quests.TryGetNextToUnlock(player);

            result.IsSome.Should().BeTrue();
            result.IfSome(quest =>
                quest.Should().Be(lastQuest));
        }
    }

    public sealed class Given_next_is_out_of_range
    {
        [Fact]
        public void Then_no_quest_is_returned()
        {
            var player = new Player()
            {
                QuestsUnlockedCount = Quests.Count
            };

            var result = Quests.TryGetNextToUnlock(player);

            result.IsNone.Should().BeTrue();
        }
    }
}