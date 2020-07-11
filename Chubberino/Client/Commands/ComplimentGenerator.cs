using System;
using System.Collections.Generic;
using System.Text;

namespace Chubberino.Client.Commands
{
    internal sealed class ComplimentGenerator
    {
        private static List<String> Compliments { get; } = new List<String>()
        {
            "Any team would be lucky to have you on it",
            "Being around you is like a happy little vacation",
            "Being around you makes everything better",
            "Being around you makes everything better",
            "Everything would be better if more people were like you",
            "I appreciate you",
            "I hope you're having a great day",
            "I wish the best for you",
            "Keep being awesome",
            "On a scale from 1 to 10, you're an 11",
            "Our community is better because you're in it",
            "Thank you for being you",
            "The people you love are lucky to have you in their lives",
            "You are awesome",
            "You are brave",
            "You are inspiring",
            "You are loved",
            "You are making a difference",
            "You are strong",
            "You are the most perfect you there is",
            "You bring out the best in people",
            "You deserve a huge right now",
            "You should be proud of yourself",
            "You're a candle in the darkness",
            "You're a gift to those around you",
            "You're a great example to others",
            "You're a smart cookie",
            "You're better than a triple scoop ice cream cone... with sprinkes",
            "You're even better than a unicorn because you're real",
            "You're like sunshine on a rainy day",
            "You're most fun than bubble wrap",
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
            "YEP",
            "esfandL",
            "iLOVEyou",
            "peepoClap",
            "peepoLove",
            "pepeD",
            "pepeJAM",
            "pepeJAMJAM",
            "pepeJAMMER",
            "prideHeartL prideHeartR",
            "prideLove",
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
