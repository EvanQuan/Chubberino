using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame
{
    public sealed class CheeseRepository : ICheeseRepository
    {
        private const Int32 CheeseUnlockedOffset = 6;

        private static IReadOnlyList<CheeseType> Cheeses { get; } = new List<CheeseType>()
        {
            new CheeseType("Stinking Bishop", -6),
            new CheeseType("Blue Castello", -2),

            new CheeseType("American Cheese", 2),
            new CheeseType("Cheddar", 6),
            new CheeseType("Mozzarella", 10),
            new CheeseType("Swiss", 14),

            // Unlockable cheese types
            new CheeseType("Gouda", 18),
            new CheeseType("Stilton", 22),
            new CheeseType("Brie", 26),
            new CheeseType("Mascarpone", 30),
            new CheeseType("Gorgonzola", 34),
            new CheeseType("Garroxta", 38),
            new CheeseType("Kashkaval", 42),
            new CheeseType("Myzithra", 46),
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
