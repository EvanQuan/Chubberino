using Chubberino.Client.Commands.Settings.ColorSelectors;
using Moq;
using System;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.ColorSelectors
{
    public abstract class UsingPresetColorSelector
    {
        protected PresetColorSelector Sut { get; }

        protected Mock<Random> MockedRandom { get; }

        public UsingPresetColorSelector()
        {
            MockedRandom = new Mock<Random>().SetupAllProperties();

            Sut = new PresetColorSelector(MockedRandom.Object);
        }
    }
}
