using System;
using System.Collections.Generic;
using Chubberino.Common.Extensions;
using Chubberino.UnitTestQualityTools.Extensions;
using Moq;
using Xunit;

namespace Chubberino.Common.UnitTests.Extensions.RandomExtensions
{
    public sealed class WhenGettingNextElement
    {
        private Mock<Random> Random { get; }

        private IReadOnlyList<Int32> List { get; } = new List<Int32>
        {
            0,
            1,
            2,
            3,
            4
        };

        public WhenGettingNextElement()
        {
            Random = new Mock<Random>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        public void ShouldReturnFirstElement(Int32 maxIndex)
        {
            Random.SetupReturnMinimum();

            Int32 result = Random.Object.NextElement(List, maxIndex);

            Assert.Equal(List[0], result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        public void ShouldReturnElementMaxIndex(Int32 maxIndex)
        {
            Random.SetupReturnMaximum();

            Int32 result = Random.Object.NextElement(List, maxIndex);

            Assert.Equal(List[maxIndex], result);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        public void ShouldReturnElementAtLastIndex(Int32 maxIndex)
        {
            Random.SetupReturnMaximum();

            Int32 result = Random.Object.NextElement(List, maxIndex);

            Assert.Equal(List[List.Count - 1], result);
        }
    }
}
