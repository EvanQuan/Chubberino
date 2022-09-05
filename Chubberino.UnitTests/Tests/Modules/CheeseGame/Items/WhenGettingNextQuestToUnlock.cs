using Chubberino.Bots.Channel.Modules.CheeseGame.Items;
using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Database.Models;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items;

public sealed class WhenGettingNextQuestToUnlock
{
    private QuestLocation Sut { get; }

    private IQuestRepository Repository { get; }

    private Player Player { get; }

    public WhenGettingNextQuestToUnlock()
    {
        Repository = new QuestRepository();

        Player = new Player();

        Sut = new QuestLocation(Repository);
    }

    [Fact]
    public void ShouldReturnFirstQuest()
    {
        var prompt = Sut.GetShopPrompt(Player);

        Assert.Contains(Repository.CommonQuests[0].Location, prompt);
    }

    /// <summary>
    /// When there are no more quests to unlock, the prompt should be null.
    /// </summary>
    [Fact]
    public void ShouldReturnNull()
    {
        Player.QuestsUnlockedCount = Repository.CommonQuests.Count;

        var prompt = Sut.GetShopPrompt(Player);

        Assert.Null(prompt);
    }

}
