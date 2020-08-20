using System;
using System.Collections.Generic;

namespace Chubberino.Client.Commands
{
    internal sealed class ComplimentGenerator
    {
        private static List<String> Compliments { get; } = new List<String>()
        {
            "Everyone loves you",
            "Everything would be better if more people were like you",
            "Have a great day",
            "I appreciate you",
            "I care about you",
            "I hope you're having a great day",
            "I wish the best for you",
            "I'm proud of you",
            "Keep being awesome",
            "Our community is better because you're in it",
            "Remember you are loved",
            "Thank you for being you",
            "The people you love are lucky to have you in their lives",
            "You are loved",
            "You are the most perfect you there is",
            "You bring out the best in people",
            "You deserve a huge hug right now",
            "You should be proud of yourself",
            "You're awesome",
            "You're great",
            "You're inspiring",
            "You're making a difference",
            "You're one of a kind",
            "You're someone's reason to smile",
            "You're wonderful",
        };

        private static List<String> Endings { get; } = new List<String>()
        {
            "<3",
            "FeelsGoodMan",
            "OkayChamp",
            "PETTHESIMBA",
            "PogU",
            "PrideHeartL PrideHeartR",
            "PrideLove",
            "PridePog",
            "PrideUwu",
            "YEP",
            "esfandL",
            "iLOVEyou",
            "peepoClap",
            "peepoLove",
            "pepeD",
            "pepeJAM",
            "pepeJAMJAM",
            "pepeJAMMER",
            "simClap",
            "simHappy",
            "simLove",
            "simkiPat",
            "widepeepoHappy",
            "yyjAYAYA",
            "yyjBloodTrail",
            "yyjCozy",
            "yyjHeart",
            "yyjHi",
            "yyjHug",
            "yyjHype",
            "yyjLove",
            "yyjPat",
            "yyjSmile",
            "yyjYou",
        };

        private Random Random { get; }

        public ComplimentGenerator()
        {
            Random = new Random();
        }

        public String GetCompliment()
        {
            String compliment = Compliments[Random.Next(Compliments.Count)];
            String ending = Endings[Random.Next(Endings.Count)];

            return $"{compliment}! {ending}";
        }
    }
}
