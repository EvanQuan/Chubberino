using System;
using Chubberino.Common.Extensions;
using Xunit;

namespace Chubberino.Common.UnitTests.Extensions.StringExtensions
{
    public sealed class WhenTryParsingEnum
    {
        public enum TestEnum
        {
            Alpha,
            Beta,
            Apple
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData(null)]
        public void ShouldReturnFalseForInt32(String source)
        {
            Boolean result = source.TryParseEnum(out Int32 value);

            Assert.False(result);
            Assert.Equal(default, value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("c")]
        [InlineData(null)]
        public void ShouldReturnFalseForNonMatchingString(String source)
        {
            Boolean result = source.TryParseEnum(out TestEnum value);

            Assert.False(result);
            Assert.Equal(default, value);
        }

        [Theory]
        [InlineData("a", TestEnum.Alpha)]
        [InlineData("al", TestEnum.Alpha)]
        [InlineData("alpha", TestEnum.Alpha)]
        [InlineData("Alpha", TestEnum.Alpha)]
        [InlineData("alPha", TestEnum.Alpha)]
        [InlineData("b", TestEnum.Beta)]
        [InlineData("beta", TestEnum.Beta)]
        [InlineData("ap", TestEnum.Apple)]
        [InlineData("apple", TestEnum.Apple)]
        public void ShouldReturnTrueForMatchingString(String source, TestEnum expectedEnum)
        {
            Boolean result = source.TryParseEnum(out TestEnum value);

            Assert.True(result);
            Assert.Equal(expectedEnum, value);
        }
    }
}
