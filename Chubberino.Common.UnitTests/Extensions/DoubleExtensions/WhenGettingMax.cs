using System.Linq;
using Chubberino.Common.Extensions;

namespace Chubberino.Common.UnitTests.Extensions.DoubleExtensions;

public sealed class WhenGettingMax
{
    [Theory]
    [InlineData(0, 0.5, 0.5)]
    [InlineData(0.5, 0, 0.5)]
    [InlineData(-0.5, 0, 0)]
    [InlineData(0, -0.5, 0)]
    [InlineData(1, 0.5, 1)]
    [InlineData(0.5, 1, 1)]
    [InlineData(-1, 1, 1)]
    [InlineData(1, -1, 1)]
    public void ShouldReturnMax(Double left, Double right, Double expectedMax)
    {
        var result = left.Max(right);

        Assert.Equal(expectedMax, result);
    }
}
