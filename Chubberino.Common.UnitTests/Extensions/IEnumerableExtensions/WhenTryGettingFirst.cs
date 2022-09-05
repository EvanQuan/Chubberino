using System;
using System.Collections.Generic;
using Chubberino.Common.Extensions;
using Xunit;

namespace Chubberino.Common.UnitTests.Extensions.IEnumerableExtensions;

public sealed class WhenTryGettingFirst
{

    [Fact]
    public void ShouldFailForInt32()
    {
        var list = new List<Int32>()
        {
            1,
            3
        };

        var result = list.TryGetFirst(x => x == 2, out var element);

        Assert.False(result);
        Assert.Equal(default, element);
    }

    [Fact]
    public void ShouldFailForTimeSpan()
    {
        var list = new List<TimeSpan>()
        {
            TimeSpan.Zero,
            TimeSpan.FromSeconds(2)
        };

        var result = list.TryGetFirst(x => x == TimeSpan.FromSeconds(1), out var element);

        Assert.False(result);
        Assert.Equal(default, element);
    }

    [Fact]
    public void ShouldFailForString()
    {
        var list = new List<String>()
        {
            String.Empty,
            "b"
        };

        var result = list.TryGetFirst(x => x == "a", out var element);

        Assert.False(result);
        Assert.Equal(default, element);
    }

    [Fact]
    public void ShouldSucceedForInt32()
    {
        var list = new List<Int32>()
        {
            1,
            3
        };

        var result = list.TryGetFirst(x => x > 1, out var element);

        Assert.True(result);
        Assert.Equal(3, element);
    }

    [Fact]
    public void ShouldSucceedForTimeSpan()
    {
        var list = new List<TimeSpan>()
        {
            TimeSpan.Zero,
            TimeSpan.FromSeconds(2)
        };

        var result = list.TryGetFirst(x => x > TimeSpan.FromSeconds(1), out var element);

        Assert.True(result);
        Assert.Equal(TimeSpan.FromSeconds(2), element);
    }

    [Fact]
    public void ShouldSucceedForString()
    {
        var list = new List<String>()
        {
            String.Empty,
            "b"
        };

        var result = list.TryGetFirst(x => x.Length > 0, out var element);

        Assert.True(result);
        Assert.Equal("b", element);
    }
}
