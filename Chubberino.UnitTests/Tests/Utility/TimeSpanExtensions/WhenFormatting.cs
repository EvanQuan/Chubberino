using System;
using Xunit;
using Chubberino.Utility;

namespace Chubberino.UnitTests.Tests.Utility.TimeSpanExtensions
{
    public sealed class WhenFormatting
    {
        [Theory]
        [InlineData(0, 0, 0, "0 seconds")]
        [InlineData(0, 0, 1, "1 second")]
        [InlineData(0, 0, 2, "2 seconds")]
        [InlineData(0, 1, 0, "1 minute")]
        [InlineData(0, 2, 0, "2 minutes")]
        [InlineData(1, 0, 0, "1 hour")]
        [InlineData(2, 0, 0, "2 hours")]
        [InlineData(1, 1, 0, "1 hour and 1 minute")]
        [InlineData(1, 2, 0, "1 hour and 2 minutes")]
        [InlineData(1, 1, 1, "1 hour, 1 minute and 1 second")]
        [InlineData(2, 2, 2, "2 hours, 2 minutes and 2 seconds")]
        public void ShouldFormat(Int32 hours, Int32 minutes, Int32 seconds, String expectedResult)
        {
            TimeSpan timespan = new TimeSpan(hours, minutes, seconds);

            var result = timespan.Format();

            Assert.Equal(expectedResult, result);
        }
    }
}
