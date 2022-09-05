using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Database.Models;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Quests.QuestRepositories;

public sealed class WhenTryGettingNextQuestToUnlock
{
    public IQuestRepository QuestRepository { get; }

    private Player Player { get; }

    public WhenTryGettingNextQuestToUnlock()
    {
        QuestRepository = new QuestRepository();
        Player = new Player();
    }

    [Fact]
    public void ShouldReturnFirstQuest()
    {
        var result = QuestRepository.CommonQuests.TryGetNextToUnlock(Player, out var quest);

        Assert.True(result);
        Assert.NotNull(quest);
        Assert.Equal(QuestRepository.CommonQuests[0], quest);
    }

    [Fact]
    public void ShouldReturnLastCheese()
    {
        Player.QuestsUnlockedCount = QuestRepository.CommonQuests.Count - 1;
        var result = QuestRepository.CommonQuests.TryGetNextToUnlock(Player, out var quest);

        Assert.True(result);
        Assert.NotNull(quest);
        Assert.Equal(QuestRepository.CommonQuests[QuestRepository.CommonQuests.Count - 1], quest);
    }

    [Fact]
    public void ShouldReturnNoCheese()
    {
        Player.QuestsUnlockedCount = QuestRepository.CommonQuests.Count;
        var result = QuestRepository.CommonQuests.TryGetNextToUnlock(Player, out var cheeseType);

        Assert.False(result);
        Assert.Null(cheeseType);
    }
}