﻿using Chubberino.Utility;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Utility.StringExtensions
{
    public sealed class WhenStrippingStart
    {
        [Theory]
        [InlineData("ab", "a", "b")]
        [InlineData("a", "a", "")]
        [InlineData("", "a", "")]
        [InlineData("b", "a", "b")]
        [InlineData("b", "", "b")]
        [InlineData("", "", "")]

        public void ShouldStripStartString(String source, String prefix, String expectedResult)
        {
            var result = source.StripStart(prefix);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("ab", 'a', "b")]
        [InlineData("a", 'a', "")]
        [InlineData("", 'a', "")]
        [InlineData("b", 'a', "b")]

        public void ShouldStripStartChar(String source, Char prefix, String expectedResult)
        {
            var result = source.StripStart(prefix);

            Assert.Equal(expectedResult, result);
        }
    }
}
