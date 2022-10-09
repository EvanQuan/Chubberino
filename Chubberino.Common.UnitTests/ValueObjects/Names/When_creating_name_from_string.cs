using Chubberino.Common.ValueObjects;

namespace Chubberino.Common.UnitTests.ValueObjects.Names;

public sealed class When_creating_name_from_string
{
    public sealed class Given_string_does_not_contain_any_uppercase_characters
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("1")]
        [InlineData("&")]
        [InlineData("q *")]
        public void Then_string_reference_is_directly_used(String value)
        {
            var result = Name.From(value);

            result.Value.Should().BeSameAs(value);
        }
    }

    public sealed class Given_string_is_null
    {
        [Fact]
        public void Then_argument_null_exception_is_thrown()
        {
            var createName = () => Name.From(null);

            createName.Should().Throw<ArgumentNullException>();
        }
    }

    public sealed class Given_string_contains_uppercase_characters
    {
        [Theory]
        [InlineData("A", "a")]
        [InlineData("ABC", "abc")]
        [InlineData("a B", "a b")]
        [InlineData("a B$", "a b$")]
        public void Then_lowercase_string_is_used(String value, String expectedResult)
        {
            var result = Name.From(value);

            result.Value.Should().Be(expectedResult);
        }
    }

}
