using System.Collections.Generic;
using Chubberino.Common.Extensions;

namespace Chubberino.Common.UnitTests.Extensions.ListExtensions;

public sealed class When_try_getting_element_from_read_only_list
{
    private static IReadOnlyList<Int32> List { get; } = new List<Int32>
    {
        0,
        1,
        2,
        3,
        4
    };

    public sealed class Given_the_index_is_outside_of_range
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(5)]
        [InlineData(6)]
        public void Then_none_is_returned(Int32 index)
        {
            var result = List.TryGet(index);

            result.IsNone.Should().BeTrue();
        }
    }

    public sealed class Given_the_index_is_in_range
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Then_element_is_returned(Int32 index)
        {
            var result = List.TryGet(index);

            result.IsSome.Should().BeTrue();
            result.IfSome(element => element.Should().Be(List[index]));
        }
    }
}
