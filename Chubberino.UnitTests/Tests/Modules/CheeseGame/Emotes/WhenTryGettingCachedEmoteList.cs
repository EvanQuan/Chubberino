using Chubberino.Database.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Emotes
{
    public sealed class WhenTryGettingCachedEmoteList : UsingEmoteManager
    {
        [Theory]
        [InlineData("", EmoteCategory.Invalid)]
        [InlineData("", EmoteCategory.Positive)]
        public void ShouldReturnNothingOnEmptyCachedEmotes(
            String channelName,
            EmoteCategory category)
        {
            var result = Sut.TryGetCachedEmoteList(channelName, category)();

            Assert.False(result.HasValue);
        }

        [Theory]
        [InlineData("", EmoteCategory.Invalid)]
        [InlineData("", EmoteCategory.Positive)]
        public void ShouldReturnNothingOnEmptyCategoryList(
            String channelName,
            EmoteCategory category)
        {
            Sut.CachedEmotes.TryAdd(channelName, new());
            var result = Sut.TryGetCachedEmoteList(channelName, category)();

            Assert.False(result.HasValue);
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

            List<String> emoteList = new();
            emoteList.Add(name);

            categoryList[category] = emoteList; 

            Sut.CachedEmotes.TryAdd(channelName, categoryList);
            var result = Sut.TryGetCachedEmoteList(channelName, category)();

            Assert.True(result.HasValue);
            Assert.Equal(emoteList, result.Value);
        }
    }
}
