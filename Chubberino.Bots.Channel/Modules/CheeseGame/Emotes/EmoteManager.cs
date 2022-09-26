using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Chubberino.Common.Extensions;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using LanguageExt;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Emotes;

public sealed class EmoteManager : IEmoteManager
{
    /// <summary>
    /// Key: channel display name
    /// Value:
    ///     Key: Emote category
    ///     Value: List of emotes for that category and channel
    /// </summary>
    public ConcurrentDictionary<String, ConcurrentDictionary<EmoteCategory, List<String>>> CachedEmotes { get; set; }

    private static IReadOnlyDictionary<EmoteCategory, IReadOnlyList<String>> DefaultEmotes { get; } = new Dictionary<EmoteCategory, IReadOnlyList<String>>()
    {
        {
            EmoteCategory.Positive,
            new List<String>()
            {
                ":)",
                ":D",
                ":p",
                ";)",
                ";p",
                "4Head",
                "<3",
                "BatChest",
                "BloodTrail",
                "CoolCat",
                "DendiFace",
                "FutureMan",
                "GivePLZ",
                "GunRun",
                "Kreygasm",
                "OpieOP",
                "PartyTime",
                "PogChamp",
                "SeemsGood",
                "TakeNRG",
                "TehePelo",
                "ThankEgg",
                "ThunBeast",
                "TwitchUnity",
                "VirtualHug",
                "bleedPurple",
            }
        },
        {
            EmoteCategory.Negative,
            new List<String>()
            {
                ":(",
                ":\\",
                ">(",
                "BabyRage",
                "BibleThump",
                "DansGame",
                "EleGiggle",
                "FUNgineer",
                "FailFish",
                "FreakinStinkin",
                "Jebaited",
                "LUL",
                "NotLikeThis",
                "PJSalt",
                "PunOko",
                "RlyTho",
                "SoBayed",
                "SwiftRage",
                "TearGlove",
                "UWot",
                "UnSane",
                "WutFace",
                "YouWHY",
            }
        },
        {
            EmoteCategory.Waiting,
            new List<String>()
            {
                "O_o",
                "BegWan",
                "CoolStoryBob",
                "ResidentSleeper",
            }
        },
        {
            EmoteCategory.Rat,
            new List<String>()
            {
                "Mau5",
            }
        },
        {
            EmoteCategory.Cat,
            new List<String>()
            {
                "CoolCat",
                "DxCat",
                "GlitchCat",
                "Keepo",
                "Kippa",
            }
        },
    };

    public IApplicationContextFactory ContextFactory { get; }

    public Random Random { get; }

    public EmoteManager(IApplicationContextFactory contextFactory, Random random)
    {
        ContextFactory = contextFactory;
        Random = random;
        CachedEmotes = new ConcurrentDictionary<String, ConcurrentDictionary<EmoteCategory, List<String>>>();
    }


    public IReadOnlyList<String> Get(String channel, EmoteCategory category)
    {
        return TryGetCachedEmoteList(channel, category)
            .Match(
                Some: cachedEmoteList => cachedEmoteList,
                None: () =>
                    TryGetAndCacheDatabaseEmoteList(channel, category)
                        .Match(
                            Some: databaseEmoteList => databaseEmoteList,
                            None: () => DefaultEmotes[category]));
    }

    public Option<IReadOnlyList<String>> TryGetCachedEmoteList(String channelName, EmoteCategory category)
    {
        if (CachedEmotes.TryGetValue(channelName, out var categoryList))
        {
            if (categoryList.TryGetValue(category, out List<String> emoteList))
            {
                return emoteList.AsReadOnly();
            }
        }

        return Option<IReadOnlyList<String>>.None;
    }

    public Option<IReadOnlyList<String>> TryGetAndCacheDatabaseEmoteList(String channelName, EmoteCategory category, IApplicationContext context = default)
    {
        context ??= ContextFactory.GetContext();

        var categoryList = CachedEmotes.GetOrAdd(channelName, _ => new ConcurrentDictionary<EmoteCategory, List<String>>());

        IQueryable<String> emotes = context
            .Emotes
            .Where(x => x.TwitchDisplayName == channelName && x.Category == category)
            .Select(x => x.Name);

        categoryList.Remove(category, out _);

        if (emotes.Any())
        {
            var databaseEmoteList = emotes.ToList().AsReadOnly();
            var categoryEmotes = categoryList.GetOrAdd(category, _ => new List<String>());

            foreach (var emote in databaseEmoteList)
            {
                categoryEmotes.Add(emote);
            }

            return databaseEmoteList;
        }

        return Option<IReadOnlyList<String>>.None;
    }

    public EmoteManagerResult AddAll(IEnumerable<String> emotes, EmoteCategory category, String channel)
    {
        using var context = ContextFactory.GetContext();

        IQueryable<String> databaseEmotes = context.Emotes
            .Where(x => x.TwitchDisplayName == channel && x.Category == category)
            .Select(x => x.Name);

        List<String> succeeded = new();
        List<String> failed = new();

        foreach (String emote in emotes)
        {
            if (databaseEmotes.Any(x => x == emote))
            {
                failed.Add(emote);
            }
            else
            {
                context.Emotes.Add(new Emote()
                {
                    Name = emote,
                    TwitchDisplayName = channel,
                    Category = category,
                });

                succeeded.Add(emote);
            }
        }

        if (succeeded.Any())
        {
            context.SaveChanges();
            TryGetAndCacheDatabaseEmoteList(channel, category, context);
        }

        return new EmoteManagerResult(succeeded, failed);
    }

    public EmoteManagerResult RemoveAll(IEnumerable<String> emotes, EmoteCategory category, String channel)
    {
        using var context = ContextFactory.GetContext();

        IQueryable<String> databaseEmotes = context
            .Emotes
            .Where(x => x.TwitchDisplayName == channel && x.Category == category)
            .Select(x => x.Name);

        List<String> succeeded = new();
        List<String> failed = new();

        foreach (String emote in emotes)
        {
            if (context.Emotes.TryGetFirst(x => x.Name == emote && x.Category == category, out Emote found))
            {
                context.Emotes.Remove(found);
                succeeded.Add(emote);
            }
            else
            {
                failed.Add(emote);
            }
        }

        if (succeeded.Any())
        {
            context.SaveChanges();
            TryGetAndCacheDatabaseEmoteList(channel, category, context);
        }

        return new EmoteManagerResult(succeeded, failed);
    }
}
