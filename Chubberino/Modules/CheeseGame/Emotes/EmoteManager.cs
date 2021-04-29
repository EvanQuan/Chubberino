using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Modules.CheeseGame.Emotes
{
    public sealed class EmoteManager : IEmoteManager
    {
        public EmoteManager(IApplicationContext context, Random random)
        {
            Context = context;
            Random = random;
            CachedEmotes = new ConcurrentDictionary<String, ConcurrentDictionary<EmoteCategory, IList<String>>>();
        }

        /// <summary>
        /// Key: channel display name
        /// Value:
        ///     Key: Emote category
        ///     Value: List of emotes for that category and channel
        /// </summary>
        public ConcurrentDictionary<String, ConcurrentDictionary<EmoteCategory, IList<String>>> CachedEmotes { get; set; }

        private static IList<String> PositiveGlobalEmotes { get; } = new List<String>()
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
            "GivePLZ",
            "GunRun",
            "Kreygasm",
            "OpieOP",
            "PartyTime",
            "PogChamp",
            "SeemsGood",
            "TakeNRG",
            "ThunBeast",
            "TwitchUnity",
            "VirtualHug",
            "bleedPurple",
        };

        private static IList<String> NegativeGlobalEmotes { get; } = new List<String>()
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
            "PJSalt",
            "PunOko",
            "SwiftRage",
            "TearGlove",
            "UWot",
            "UnSane",
            "WutFace",
            "YouWHY",
        };

        public IApplicationContext Context { get; }

        public Random Random { get; }

        public String GetRandomPositiveEmote(String channelName)
        {
            var emoteList = GetEmoteList(channelName, EmoteCategory.Positive);
            return Random.NextElement(emoteList);
        }

        public String GetRandomNegativeEmote(String channelName)
        {
            var emoteList = GetEmoteList(channelName, EmoteCategory.Negative);
            return Random.NextElement(emoteList);
        }

        private IList<String> GetEmoteList(String channelName, EmoteCategory category)
        {
            return TryGetCachedEmoteList(channelName, category, out var cachedEmoteList)
                ? cachedEmoteList
                : TryGetAndCacheDatabaseEmoteList(channelName, category, out var databaseEmoteList)
                    ? databaseEmoteList
                    : GetDefaultEmoteList(category);
        }

        private static IList<String> GetDefaultEmoteList(EmoteCategory category)
        {
            return category switch
            {
                EmoteCategory.Positive => PositiveGlobalEmotes,
                EmoteCategory.Negative => NegativeGlobalEmotes,
                _ => default
            };
        }

        private Boolean TryGetCachedEmoteList(String channelName, EmoteCategory category, out IList<String> cachedEmoteList)
        {
            if (CachedEmotes.TryGetValue(channelName, out var categoryList))
            {
                if (categoryList.TryGetValue(category, out var emoteList))
                {
                    cachedEmoteList = emoteList;
                    return true;
                }
            }

            cachedEmoteList = default;
            return false;
        }

        private Boolean TryGetAndCacheDatabaseEmoteList(String channelName, EmoteCategory category, out IList<String> databaseEmoteList)
        {
            var categoryList = CachedEmotes.GetOrAdd(channelName, _ => new ConcurrentDictionary<EmoteCategory, IList<String>>());

            IQueryable<String> emotes = Context.Emotes
                .Where(x => x.TwitchDisplayName == channelName && x.Category == category)
                .Select(x => x.Name);

            if (emotes.Any())
            {
                databaseEmoteList = emotes.ToList();
                var categoryEmotes = categoryList.GetOrAdd(category, _ => new List<String>());

                foreach (var emote in databaseEmoteList)
                {
                    categoryEmotes.Add(emote);
                }

                return true;
            }

            categoryList.Remove(category, out _);
            databaseEmoteList = default;
            return false;
        }

        public void Refresh(String channel)
        {
            foreach (EmoteCategory category in Enum.GetValues<EmoteCategory>().Skip(1))
            {
                TryGetAndCacheDatabaseEmoteList(channel, category, out _);
            }
        }

        public void Add(String emote, EmoteCategory category, String channel)
        {

            Context.Emotes.Add(new Emote()
            {
                Name = emote,
                TwitchDisplayName = channel,
                Category = category,
            });
            Context.SaveChanges();

            TryGetAndCacheDatabaseEmoteList(channel, category, out _);
        }

        public Boolean TryRemove(String emote, EmoteCategory category, String channel)
        {
            if (Context.Emotes.TryGetFirst(x => x.Name == emote && x.Category == category, out Emote found))
            {
                Context.Emotes.Remove(found);
                Context.SaveChanges();
                TryGetAndCacheDatabaseEmoteList(channel, category, out _);
                return true;
            };

            return false;
        }
    }
}
