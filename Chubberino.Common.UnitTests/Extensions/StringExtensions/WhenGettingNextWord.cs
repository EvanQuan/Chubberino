﻿using Chubberino.Common.Extensions;

namespace Chubberino.Common.UnitTests.Extensions.StringExtensions;

public sealed class WhenGettingNextWord
{
    [Theory]
    [InlineData("a", "a", "")]
    [InlineData("a b", "a", "b")]
    [InlineData("", "", "")]
    [InlineData(" ", "", "")]
    [InlineData(" a", "a", "")]
    [InlineData(" a b", "a", "b")]
    [InlineData(" a ", "a", "")]
    public void ShouldGetNextWord(String source, String expectedWord, String expectedRemainder)
    {
        String remainder = source.GetNextWord(out String word);

        Assert.Equal(expectedWord, word);
        Assert.Equal(expectedRemainder, remainder);
    }
}
