using System;
using Chubberino.Common.ValueObjects;
using Xunit;

namespace Chubberino.Common.UnitTests.ValueObjects.LowercaseStrings
{
    public sealed class WhenCreatingFromString
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("1")]
        [InlineData("&")]
        [InlineData("q *")]
        public void ShouldCreateSuccessfully(String value)
        {
            var result = LowercaseString.From(value);

            Assert.Equal(result.ToString(), value);
        }

        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                LowercaseString.From(null);
            });
        }

        [Theory]
        [InlineData("A")]
        [InlineData("ABC")]
        [InlineData("a B")]
        [InlineData("a B$")]
        public void ShouldThrowFormatException(String value)
        {
            var exception = Assert.Throws<FormatException>(() =>
            {
                LowercaseString.From(value);
            });

            Assert.Equal(String.Format(LowercaseString.FormatExceptionMesage, value), exception.Message);
        }
    }
}
