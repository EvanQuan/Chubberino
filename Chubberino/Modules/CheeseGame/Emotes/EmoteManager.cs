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

        private static IReadOnlyList<String> PositiveEmotes { get; } = new List<String>()
        {
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
            "berryA",
            "chubDuane",
            "elmoHype",
            "iron95Pls",
            "peepoClap",
            "pepoBoogie",
            "sanaSnuggle",
            "sheCrazy",
            "vibePls",
            "yyjSUPERPOG",
            "yyjTasty",
        };

        private static IReadOnlyList<String> NegativeEmotes { get; } = new List<String>()
        {
            "4Weirder",
            "ChubOest",
            "DansChamp",
            "Jelleh",
            "KEKBye",
            "KEKW",
            "Karen",
            "LULW",
            "NOIDONTTHINKSO",
            "NOP",
            "NOPERS",
            "OMEGALUL",
            "PainChamp",
            "PepeLaugh",
            "PepeSpit",
            "PogO",
            "Sadge",
            "TearChub",
            "WeirdChamp",
            "WideRage",
            "chubLeave",
            "chubOff",
            "reeferSad",
            "widepeepoLuL",
            "widepeepoSad",
            "yyjOMEGALULDANCE",
            "ZULUL",
        };
        public Random Random { get; }

        public String GetRandomPositiveEmote()
        {
            return PositiveEmotes[Random.Next(PositiveEmotes.Count)];
        }

        public String GetRandomNegativeEmote()
        {
            return NegativeEmotes[Random.Next(NegativeEmotes.Count)];
        }
    }
}
