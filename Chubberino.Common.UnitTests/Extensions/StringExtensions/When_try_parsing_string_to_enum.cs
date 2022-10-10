using Chubberino.Common.Extensions;

namespace Chubberino.Common.UnitTests.Extensions.StringExtensions;

public sealed class When_try_parsing_string_to_enum
{
    public enum TestEnum
    {
        Alpha,
        Beta,
        Apple
    }

    public sealed class Given_the_input_matches_a_defined_value
    {
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
        public void Then_matching_enum_value_is_returned(String source, TestEnum expectedEnum)
        {
            var result = source.TryParseEnum<TestEnum>();

            result.IsSome.Should().BeTrue();
            result.IfSome(value => value.Should().Be(expectedEnum));
        }
    }

    public sealed class Given_the_input_does_not_match_any_defined_value
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("c")]
        [InlineData(null)]
        public void Then_none_is_returned(String source)
        {
            var result = source.TryParseEnum<TestEnum>();

            result.IsNone.Should().BeTrue();
        }
    }
}
