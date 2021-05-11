using Chubberino.Utility;
using System;
using System.Collections.Generic;
using Xunit;

namespace Chubberino.UnitTests.Tests.Utility.ListExtensions
{
    public sealed class WhenTryGettingElement
    {
        private IList<Int32> List { get; } = new List<Int32>
        {
            0,
            1,
            2,
            3,
            4
        };

        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(5)]
        [InlineData(6)]
        public void ShouldFailToGetElement(Int32 index)
        {
            var result = List.TryGet(index, out Int32 element);

            Assert.False(result);
            Assert.Equal(default, element);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ShouldSucceedToGetElement(Int32 index)
        {
            var result = List.TryGet(index, out Int32 element);

            Assert.True(result);
            Assert.Equal(List[index], element);
        }
    }
}
