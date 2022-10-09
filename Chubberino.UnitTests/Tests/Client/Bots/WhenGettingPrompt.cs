namespace Chubberino.UnitTests.Tests.Client.Bots;

public sealed class WhenGettingPrompt : UsingBot
{
    [Theory]
    [InlineData(true, "Mod")]
    [InlineData(false, "Normal")]
    public void ShouldContainRelevantInformation(Boolean isModerator, String expectedModeratorText)
    {
        Sut.IsModerator = isModerator;

        var commandStatus = Guid.NewGuid().ToString();

        MockedCommandRepository.Setup(x => x.GetStatus()).Returns(commandStatus);

        MockedTwitchClientManager.Setup(x => x.PrimaryChannelName).Returns(Guid.NewGuid().ToString());

        String prompt = Sut.GetPrompt();

        Assert.Contains(commandStatus, prompt);
        Assert.Contains(expectedModeratorText, prompt);
        Assert.Contains(MockedTwitchClientManager.Object.PrimaryChannelName, prompt);
    }
}
