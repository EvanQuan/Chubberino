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
            "NODDERS",
            "OkayChamp",
            "POGCRAZY",
            "POGGERS",
            "Pepetusa",
            "PogU",
            "SUPERPOG",
            "WOODY",
            "berryW",
            "catJAM",
            "chubDuane",
            "chubEars",
            "chubJam",
            "chubKiss",
            "elmoHype",
            "iron95Pls",
            "kumoJAM",
            "peepoClap",
            "pepoBoogie",
            "sanaSnuggle",
            "sheCrazy",
            "vibePls",
            "yyjSUPERPOG",
            "yyjTasty",
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
            "WideRage",
            "ZULUL",
            "chubLeave",
            "chubOff",
            "reeferSad",
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
