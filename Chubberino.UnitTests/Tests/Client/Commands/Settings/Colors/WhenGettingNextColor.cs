using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Colors
{
    public sealed class WhenGettingNextColor : UsingRainbowColorSelector
    {
        [Fact]
        public void ShouldCycleThroughRainbow()
        {
            var orange = Sut.GetNextColor();
            var yellow = Sut.GetNextColor();
            var green = Sut.GetNextColor();
            var blue = Sut.GetNextColor();
            var indigo = Sut.GetNextColor();
            var violet = Sut.GetNextColor();
            var red2 = Sut.GetNextColor();
            var orange2 = Sut.GetNextColor();
            var yellow2 = Sut.GetNextColor();
            var green2 = Sut.GetNextColor();
            var blue2 = Sut.GetNextColor();
            var indigo2 = Sut.GetNextColor();
            var violet2 = Sut.GetNextColor();
            var red = Sut.GetNextColor();

            Assert.Equal("#FF0000", red);
            Assert.Equal("#FF0000", red2);

            Assert.Equal("#FF7F00", orange);
            Assert.Equal("#FF7F00", orange2);

            Assert.Equal("#FFFF00", yellow);
            Assert.Equal("#FFFF00", yellow2);

            Assert.Equal("#00FF00", green);
            Assert.Equal("#00FF00", green2);

            Assert.Equal("#0000FF", blue);
            Assert.Equal("#0000FF", blue2);

            Assert.Equal("#4B0082", indigo);
            Assert.Equal("#4B0082", indigo2);

            Assert.Equal("#9400D3", violet);
            Assert.Equal("#9400D3", violet2);
        }
    }
}
