using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Bots
{
    public sealed class WhenGettingPrompt : UsingBot
    {
        [Theory]
        [InlineData(true, "Mod")]
        [InlineData(false, "Normal")]
        public void ShouldContainRelevantInformation(Boolean isModerator, String expectedModeratorText)
        {
            BotInfo.IsModerator = isModerator;

            var commandStatus = Guid.NewGuid().ToString();

            MockedCommandRepository.Setup(x => x.GetStatus()).Returns(commandStatus);

            String prompt = Sut.GetPrompt();

            Assert.Contains(commandStatus, prompt);
            Assert.Contains(expectedModeratorText, prompt);
            Assert.Contains(BotInfo.ChannelName, prompt);
        }
    }
}
