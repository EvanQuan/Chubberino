using System;
using System.Linq;
using Chubberino.Common.Extensions;
using Xunit;

namespace Chubberino.Common.UnitTests.Extensions.DoubleExtensions
{
    public sealed class WhenGettingMin
    {
        [Theory]
        [InlineData(0, 0.5, 0)]
        [InlineData(0.5, 0, 0)]
        [InlineData(-0.5, 0, -0.5)]
        [InlineData(0, -0.5, -0.5)]
        [InlineData(1, 0.5, 0.5)]
        [InlineData(0.5, 1, 0.5)]
        [InlineData(-1, 1, -1)]
        [InlineData(1, -1, -1)]
        public void ShouldReturnMin(Double left, Double right, Double expectedMin)
        {
            var result = left.Min(right);

            Assert.Equal(expectedMin, result);
        }
    }
}
