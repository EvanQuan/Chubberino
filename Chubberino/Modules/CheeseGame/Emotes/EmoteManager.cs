using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Emotes
{
    public sealed class EmoteManager : IEmoteManager
    {
        public EmoteManager(Random random)
        {
            Random = random;
        }

        private static IReadOnlyList<String> PositiveChannelEmotes { get; } = new List<String>()
        {
            "BLANKIES",
            "Brows",
            "CHEESERS",
            "CHUBBIES",
            "COGGERS",
            "Chubbehtusa",
            "Chubbies",
            "DONKERS",
            "Kneecopter",
            "MmmHmm",
            "NODDERS",
            "OkayChamp",
            "PAGGING",
            "PETTHECHUBBEH",
            "POGCRAZY",
            "POGGERS",
            "Pepetusa",
            "PogU",
            "SUPERPOG",
            "TriDance",
            "VeryPog",
            "WICKED",
            "WOODY",
            "berryJam",
            "berryW",
            "catJAM",
            "catKiss",
            "chubDuane",
            "chubEars",
            "chubJam",
            "chubKiss",
            "docJAM",
            "elmoHype",
            "iron95Pls",
            "kittyGOGO",
            "kumoJAM",
            "peepoClap",
            "peepoCocoa",
            "peepoShy",
            "pepeD",
            "pepoBoogie",
            "rareChubbeh",
            "sanaSnuggle",
            "seoaJam",
            "sheCrazy",
            "SquirtleJam",
            "vibePls",
            "yyjSUPERPOG",
            "yyjTasty",
            "yyjTwerk",
        };

        private static IReadOnlyList<String> PositiveGlobalEmotes { get; } = new List<String>()
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

        private static IReadOnlyList<String> NegativeChannelEmotes { get; } = new List<String>()
        {
            "4Weirder",
            "ChubOest",
            "DansChamp",
            "GOTTEM",
            "Jelleh",
            "KEKBye",
            "KEKW",
            "Karen",
            "LULW",
            "MODS",
            "NOIDONTTHINKSO",
            "NOP",
            "NOPERS",
            "OMEGALUL",
            "PainChamp",
            "PepeLaugh",
            "PogO",
            "RiggedGame",
            "Sadge",
            "TearChub",
            "WeirdChamp",
            "WideRage",
            "ZULUL",
            "chubLeave",
            "chubOff",
            "fuBaldi",
            "monkaW",
            "reeferSad",
            "whatChamp",
            "widepeepoLuL",
            "widepeepoSad",
        };

        private static IReadOnlyList<String> NegativeGlobalEmotes { get; } = new List<String>()
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

        public Random Random { get; }

        public String GetRandomPositiveEmote(String channelName)
        {
            return channelName.Equals("ChubbehMouse", StringComparison.OrdinalIgnoreCase)
                ? PositiveChannelEmotes[Random.Next(PositiveChannelEmotes.Count)]
                : PositiveGlobalEmotes[Random.Next(PositiveGlobalEmotes.Count)];
        }

        public String GetRandomNegativeEmote(String channelName)
        {
            return channelName.Equals("ChubbehMouse", StringComparison.OrdinalIgnoreCase)
                ? NegativeChannelEmotes[Random.Next(NegativeChannelEmotes.Count)]
                : NegativeGlobalEmotes[Random.Next(NegativeGlobalEmotes.Count)];
        }
    }
}
