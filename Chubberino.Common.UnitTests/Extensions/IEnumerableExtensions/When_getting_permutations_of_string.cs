using System.Collections.Generic;
using System.Linq;
using Chubberino.Common.Extensions;

namespace Chubberino.Common.UnitTests.Extensions.IEnumerableExtensions;

public sealed class When_getting_permutations_of_string
{
    public sealed class Given_one_or_no_characters_exist
    {
        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("")]
        [InlineData(null)]
        public void Then_input_is_returned(String source)
        {
            IEnumerable<String> permutations = source.GetPermutations();

            permutations.Should().Equal(source);
        }
    }

    public sealed class Given_multiple_characters_exist
    {
        [Theory]
        [InlineData("aa", new String[] { "aa" })]
        [InlineData("ab", new String[] { "ab", "ba" })]
        [InlineData("abb", new String[] { "abb", "bab", "bba" })]
        [InlineData("abc", new String[] { "abc", "acb", "bac", "bca", "cab", "cba" })]
        public void Then_all_unique_permutations_are_returned(String source, String[] expected)
        {
            IEnumerable<String> permutations = source.GetPermutations();

            var result = permutations.OrderBy(value => value);

            result.Should().Equal(expected);
        }
    }
}
