using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Rankings;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class CheeseRepository : ICheeseRepository
    {
        private const Int32 CheeseUnlockedOffset = 6;

        private static IReadOnlyList<CheeseType> Cheeses { get; } = new List<CheeseType>()
        {
            new CheeseType("Stinking Bishop", -6),
            new CheeseType("Blue Castello", -2),

            new CheeseType("American", 2),
            new CheeseType("Cheddar", 6),
            new CheeseType("Mozzarella", 10),
            new CheeseType("Swiss", 14),

            // Unlockable cheese types
            new CheeseType("Gouda", 18, Rank.Bronze, 100),
            new CheeseType("Stilton", 22, Rank.Silver, 200),
            new CheeseType("Brie", 26, Rank.Gold, 400),
            new CheeseType("Mascarpone", 30, Rank.Platinum, 800),
            new CheeseType("Gorgonzola", 34, Rank.Diamond, 1600),
            new CheeseType("Garroxta", 38, Rank.Master, 3200),
            new CheeseType("Myzithra", 42, Rank.Grandmaster, 6400),
            new CheeseType("Pule", 46, Rank.Legend, 12800),
        };

        public static IReadOnlyList<CheeseVariant> Variants { get; } = new List<CheeseVariant>()
        {
            new CheeseVariant("some remains of", -1),
            new CheeseVariant("a slice of", 0),
            new CheeseVariant("a wheel of", 1),
            new CheeseVariant("a pile of", 2),
        };

        public CheeseRepository(Random random)
        {
            Random = random;
        }

        public Random Random { get; }

        public CheeseType GetNextCheeseToUnlock(Player player)
        {
            if (player.CheeseUnlocked + CheeseUnlockedOffset >= Cheeses.Count) { return null; }

            return Cheeses[CheeseUnlockedOffset + player.CheeseUnlocked];
        }

        public CheeseType GetRandomType(Int32 cheeseUnlocked)
        {
            var baseType = GetRandomBaseType(cheeseUnlocked);
            var variant = GetRandomVariant();

            return new CheeseType(variant.Name + " " + baseType.Name, Math.Sign(baseType.PointValue) * variant.PointValue + baseType.PointValue);
        }

        private CheeseType GetRandomBaseType(Int32 cheeseUnlocked)
        {
            return Cheeses[Random.Next(Math.Min(Cheeses.Count, CheeseUnlockedOffset + cheeseUnlocked))];
        }

        private CheeseVariant GetRandomVariant()
        {
            return Variants[Random.Next(Variants.Count)];
        }
    }
}
