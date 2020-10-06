using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.ColorSelectors.RandomColorSelectors
{
    public sealed class WhenGettingNextColor : UsingRandomColorSelector
    {
        [Theory]
        [InlineData(0, "#000000")]
        [InlineData(1, "#000001")]
        [InlineData(10, "#00000A")]
        [InlineData(15, "#00000F")]
        [InlineData(16, "#000010")]
        public void ShouldReturnRandomColor(Int32 randomNumber, String expectedColor)
        {
            MockedRandom.Setup(x => x.Next(0x1000000))
                .Returns(randomNumber);

            String color = Sut.GetNextColor();

            Assert.Equal(expectedColor, color);
        }
    }
}
