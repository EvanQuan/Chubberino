using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Chubberino.Database.Models;
using FluentAssertions;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Emotes;

public sealed class WhenTryGettingCachedEmoteList : UsingEmoteManager
{
    [Theory]
    [InlineData("", EmoteCategory.Invalid)]
    [InlineData("", EmoteCategory.Positive)]
    public void ShouldReturnNothingOnEmptyCachedEmotes(
        String channelName,
        EmoteCategory category)
    {
        var result = Sut.TryGetCachedEmoteList(channelName, category);

        result.IsNone.Should().BeTrue();
    }

    [Theory]
    [InlineData("", EmoteCategory.Invalid)]
    [InlineData("", EmoteCategory.Positive)]
    public void ShouldReturnNothingOnEmptyCategoryList(
        String channelName,
        EmoteCategory category)
    {
        Sut.CachedEmotes.TryAdd(channelName, new());
        var result = Sut.TryGetCachedEmoteList(channelName, category);

        result.IsNone.Should().BeTrue();
    }

    [Theory]
    [InlineData("", EmoteCategory.Invalid)]
    [InlineData("", EmoteCategory.Positive)]
    public void ShouldReturnEmoteList(
        String channelName,
        EmoteCategory category)
    {
        ConcurrentDictionary<EmoteCategory, List<String>> categoryList = new();

        String name = Guid.NewGuid().ToString();

        List<String> emoteList = new()
        {
            name
        };

        categoryList[category] = emoteList; 

        Sut.CachedEmotes.TryAdd(channelName, categoryList);
        var result = Sut.TryGetCachedEmoteList(channelName, category);

        result.IsSome.Should().BeTrue();
        result.IfSome(list => list.Should().Equal(emoteList));
    }
}
