using System;
using System.Linq;
using Chubberino.Common.Extensions;
using Xunit;

namespace Chubberino.Common.UnitTests.Extensions.StringExtensions
{
    public sealed class WhenSplittingByLengthOnWord
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ShouldThrowNullReferenceException(Int32 segmentLength)
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                ((String)null).SplitByLengthOnWord(segmentLength);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        public void ShouldReturnSource(String source)
        {
            var result = source.SplitByLengthOnWord(10);

            Assert.Single(result);

            Assert.Equal(result.First(), source);
        }

        [Theory]
        [InlineData("0 1 2", -1, new String[] { "0", "1", "2" })]
        [InlineData("0 1 2", 0, new String[] { "0", "1", "2" })]
        [InlineData("0 1 2", 1, new String[] { "0", "1", "2" })]
        [InlineData("0 1 2", 2, new String[] { "0", "1", "2" })]
        [InlineData("0 1 2", 3, new String[] { "0 1", "2" })]
        [InlineData("0 1 2", 4, new String[] { "0 1", "2" })]
        [InlineData("0 1 2", 5, new String[] { "0 1 2" })]
        [InlineData("012", 3, new String[] { "012" })]
        [InlineData("012", 4, new String[] { "012" })]
        [InlineData("012 ", 4, new String[] { "012 " })]
        [InlineData("012 ", 5, new String[] { "012 " })]
        public void ShouldReturnSegments(String source, Int32 segmentLength, String[] expectedSegments)
        {
            var result = source.SplitByLengthOnWord(segmentLength);

            Assert.Equal(expectedSegments.Length, result.Count());

            Assert.Equal(expectedSegments, result);
        }
    }
}
