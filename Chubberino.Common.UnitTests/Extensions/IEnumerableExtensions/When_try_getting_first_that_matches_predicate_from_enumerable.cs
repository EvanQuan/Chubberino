using Chubberino.Common.Extensions;

namespace Chubberino.Common.UnitTests.Extensions.IEnumerableExtensions;

public sealed class When_try_getting_first_that_matches_predicate_from_enumerable
{
    public sealed class Given_predicate_does_not_match_any_elements
    {
        [Fact]
        public void Then_none_is_returned_for_int32()
        {
            var list = new Int32[]
            {
                1,
                3
            };

            var result = list.TryGetFirst(x => x == 2);

            result.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Then_none_is_returned_for_time_span()
        {
            var list = new TimeSpan[]
            {
                TimeSpan.Zero,
                TimeSpan.FromSeconds(2)
            };

            var result = list.TryGetFirst(x => x == TimeSpan.FromSeconds(1));

            result.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Then_none_is_returned_for_string()
        {
            var list = new String[]
            {
                String.Empty,
                "b"
            };

            var result = list.TryGetFirst(x => x == "a");

            result.IsNone.Should().BeTrue();
        }
    }

    public sealed class Given_predicate_matches_an_element
    {
        [Fact]
        public void Then_first_matching_int32_is_returned()
        {
            var list = new Int32[]
            {
                1,
                5,
                3
            };

            var result = list.TryGetFirst(x => x > 1);

            result.IsSome.Should().BeTrue();
            result.IfSome(value => value.Should().Be(5));
        }

        [Fact]
        public void Then_first_matching_time_span_is_returned()
        {
            var list = new TimeSpan[]
            {
                TimeSpan.Zero,
                TimeSpan.FromSeconds(3),
                TimeSpan.FromSeconds(2),
            };

            var result = list.TryGetFirst(x => x > TimeSpan.FromSeconds(1));

            result.IsSome.Should().BeTrue();
            result.IfSome(value => value.Should().Be(TimeSpan.FromSeconds(3)));
        }

        [Fact]
        public void Then_first_matching_string_is_returned()
        {
            var list = new String[]
            {
                String.Empty,
                "b"
            };

            var result = list.TryGetFirst(x => x.Length > 0);

            result.IsSome.Should().BeTrue();
            result.IfSome(value => value.Should().Be("b"));
        }
    }
}
