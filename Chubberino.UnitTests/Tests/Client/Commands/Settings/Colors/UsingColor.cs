using Chubberino.Client.Commands.Settings;
using Chubberino.Client.Commands.Settings.ColorSelectors;
using Moq;
using System;
using TwitchLib.Client.Events;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Colors
{
    public abstract class UsingColor : UsingCommand
    {
        protected Color Sut { get; }

        protected Mock<IColorSelector> MockedSelector1 { get; }

        protected Mock<IColorSelector> MockedSelector2 { get; }

        protected String Message { get; } = Guid.NewGuid().ToString();
        protected String Username { get; } = Guid.NewGuid().ToString();
        protected String BotUsername { get; } = Guid.NewGuid().ToString();
        protected String Color1 { get; } = Guid.NewGuid().ToString();
        protected String Color2 { get; } = Guid.NewGuid().ToString();

        public UsingColor()
        {
            MockedSelector1 = new Mock<IColorSelector>().SetupAllProperties();
            MockedSelector2 = new Mock<IColorSelector>().SetupAllProperties();

            Sut = new Color(MockedTwitchClientManager.Object, MockedConsole.Object);

            MockedSelector1.Setup(x => x.GetNextColor())
                .Returns(Color1);

            MockedSelector2.Setup(x => x.GetNextColor())
                .Returns(Color2);
        }

    }
}
