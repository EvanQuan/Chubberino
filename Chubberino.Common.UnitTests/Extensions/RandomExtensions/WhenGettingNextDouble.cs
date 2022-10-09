using Chubberino.Common.Extensions;

namespace Chubberino.Common.UnitTests.Extensions.RandomExtensions;

public sealed class WhenGettingNextDouble
{
    Mock<Random> Random { get; }

    public WhenGettingNextDouble()
    {
        Random = new Mock<Random>();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(0.5, 1.5)]
    [InlineData(2, 3)]
    [InlineData(1, 0)]
    [InlineData(1.5, 0.5)]
    [InlineData(3, 2)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    public void ShouldReturnMinimum(Double minimum, Double maximum)
    {
        Random.Setup(x => x.NextDouble()).Returns(0);

        Double result = Random.Object.NextDouble(minimum, maximum);

        Assert.Equal(minimum, result);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(0.5, 1.5)]
    [InlineData(2, 3)]
    [InlineData(1, 0)]
    [InlineData(1.5, 0.5)]
    [InlineData(3, 2)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    public void ShouldReturnMaximum(Double minimum, Double maximum)
    {
        Random.Setup(x => x.NextDouble()).Returns(1);

        Double result = Random.Object.NextDouble(minimum, maximum);

        Assert.Equal(maximum, result);
    }
}
