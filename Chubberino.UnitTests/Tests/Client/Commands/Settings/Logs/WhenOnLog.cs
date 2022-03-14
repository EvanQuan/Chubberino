using Moq;
using System;
using TwitchLib.Client.Events;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Logs
{
    public sealed class WhenOnLog : UsingLog
    {
        /// <summary>
        /// Should log the data provided by <see cref="OnLogArgs"/>.
        /// </summary>
        [Fact]
        public void ShouldLogMessageData()
        {
            DateTime expectedDateTime = DateTime.Now;
            String expectedBotUserName = Guid.NewGuid().ToString();
            String expectedData = Guid.NewGuid().ToString();

            var args = new OnLogArgs()
            {
                DateTime = expectedDateTime,
                BotUsername = expectedBotUserName,
                Data = expectedData
            };

            Sut.TwitchClient_OnLog(null, args);

            MockedWriter.Verify(x => x.WriteLine($"{expectedDateTime}: {expectedBotUserName} - {expectedData}"), Times.Once());
        }
    }
}
