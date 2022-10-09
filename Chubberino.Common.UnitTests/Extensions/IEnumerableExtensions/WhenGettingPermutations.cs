﻿using System.Collections.Generic;
using System.Linq;
using Chubberino.Common.Extensions;

namespace Chubberino.Common.UnitTests.Extensions.IEnumerableExtensions;

public sealed class WhenGettingPermutations
{
    [Theory]
    [InlineData("a")]
    [InlineData("b")]
    [InlineData("")]
    public void ShouldReturnSelf(String source)
    {
        IEnumerable<String> permutations = source.GetPermutations();

        Assert.Single(permutations);
        Assert.Equal(source, permutations.First());
    }

    [Theory]
    [InlineData("ab", new String[] { "ab", "ba" })]
    [InlineData("abc", new String[] { "abc", "acb", "bac", "bca", "cab", "cba" })]
    public void ShouldReturnPermutations(String source, String[] expected)
    {
        IEnumerable<String> permutations = source.GetPermutations();

        Assert.Equal(expected.Length, permutations.Count());

        foreach (String expectedPermutation in expected)
        {
            Assert.Contains(expectedPermutation, permutations);
        }
    }


}
