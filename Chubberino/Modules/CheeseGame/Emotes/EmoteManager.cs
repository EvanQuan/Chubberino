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
            CachedEmotes = new ConcurrentDictionary<String, ConcurrentDictionary<EmoteCategory, List<String>>>();
        }

        /// <summary>
        /// Key: channel display name
        /// Value:
        ///     Key: Emote category
        ///     Value: List of emotes for that category and channel
        /// </summary>
        private ConcurrentDictionary<String, ConcurrentDictionary<EmoteCategory, List<String>>> CachedEmotes { get; set; }

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

        public IApplicationContext Context { get; }

        public Random Random { get; }

        public IReadOnlyList<String> Get(String channel, EmoteCategory category)
        {
            return TryGetCachedEmoteList(channel, category, out var cachedEmoteList)
                ? cachedEmoteList
                : TryGetAndCacheDatabaseEmoteList(channel, category, out var databaseEmoteList)
                    ? databaseEmoteList
                    : DefaultEmotes[category];
        }

        private Boolean TryGetCachedEmoteList(String channelName, EmoteCategory category, out IReadOnlyList<String> cachedEmoteList)
        {
            if (CachedEmotes.TryGetValue(channelName, out ConcurrentDictionary<EmoteCategory, List<String>> categoryList))
            {
                if (categoryList.TryGetValue(category, out List<String> emoteList))
                {
                    cachedEmoteList = emoteList.AsReadOnly();
                    return true;
                }
            }

            cachedEmoteList = default;
            return false;
        }

        private Boolean TryGetAndCacheDatabaseEmoteList(String channelName, EmoteCategory category, out IReadOnlyList<String> databaseEmoteList)
        {
            var categoryList = CachedEmotes.GetOrAdd(channelName, _ => new ConcurrentDictionary<EmoteCategory, List<String>>());

            IQueryable<String> emotes = Context.Emotes
                .Where(x => x.TwitchDisplayName == channelName && x.Category == category)
                .Select(x => x.Name);

            categoryList.Remove(category, out _);

            if (emotes.Any())
            {
                databaseEmoteList = emotes.ToList().AsReadOnly();
                var categoryEmotes = categoryList.GetOrAdd(category, _ => new List<String>());

                foreach (var emote in databaseEmoteList)
                {
                    categoryEmotes.Add(emote);
                }

                return true;
            }

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

        public EmoteManagerResult AddAll(IEnumerable<String> emotes, EmoteCategory category, String channel)
        {
            IQueryable<String> databaseEmotes = Context.Emotes
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
                    Context.Emotes.Add(new Emote()
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
                Context.SaveChanges();
                TryGetAndCacheDatabaseEmoteList(channel, category, out _);
            }

            return new EmoteManagerResult(succeeded, failed);
        }

        public EmoteManagerResult RemoveAll(IEnumerable<String> emotes, EmoteCategory category, String channel)
        {
            IQueryable<String> databaseEmotes = Context.Emotes
                .Where(x => x.TwitchDisplayName == channel && x.Category == category)
                .Select(x => x.Name);

            List<String> succeeded = new();
            List<String> failed = new();

            foreach (String emote in emotes)
            {
                if (Context.Emotes.TryGetFirst(x => x.Name == emote && x.Category == category, out Emote found))
                {
                    Context.Emotes.Remove(found);
                    succeeded.Add(emote);
                }
                else
                {
                    failed.Add(emote);
                }
            }

            if (succeeded.Any())
            {
                Context.SaveChanges();
                TryGetAndCacheDatabaseEmoteList(channel, category, out _);
            }

            return new EmoteManagerResult(succeeded, failed);
        }
    }
}
