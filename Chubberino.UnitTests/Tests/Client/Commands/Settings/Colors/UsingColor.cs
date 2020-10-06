using Chubberino.Client.Commands.Settings;
using Chubberino.Client.Commands.Settings.ColorSelectors;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Colors
{
    public abstract class UsingColor : UsingCommand
    {
        protected Color Sut { get; }

        protected Mock<IColorSelector> MockedSelector1 { get; }

        protected Mock<IColorSelector> MockedSelector2 { get; }

        public UsingColor()
        {
            MockedSelector1 = new Mock<IColorSelector>().SetupAllProperties();
            MockedSelector2 = new Mock<IColorSelector>().SetupAllProperties();

            Sut = new Color(MockedTwitchClient.Object, MockedConsole.Object);
        }

    }
}
