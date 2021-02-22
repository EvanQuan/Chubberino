using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Rankings;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class CheeseRepository : ICheeseRepository
    {
        private const Int32 CheeseUnlockedOffset = 5;

        private static IReadOnlyList<CheeseType> Cheeses { get; } = new List<CheeseType>()
        {
            new CheeseType("Stinking Bishop", -5),
            new CheeseType("Blue Castello", -1),

            new CheeseType("American", 2),
            new CheeseType("Cheddar", 4),
            new CheeseType("Mozzarella", 6),

            // Unlockable cheese types
            new CheeseType("Swiss", 8, Rank.Bronze, 75),
            new CheeseType("Babybel", 10, Rank.Bronze, 100),
            new CheeseType("Gouda", 12, Rank.Bronze, 125),
            new CheeseType("Monterey Jack", 14, Rank.Silver, 150),
            new CheeseType("Asiago", 16, Rank.Silver, 200),
            new CheeseType("Camembert", 18, Rank.Silver, 250),
            new CheeseType("Grana", 20, Rank.Gold, 325),
            new CheeseType("Brie", 22, Rank.Gold, 400),
            new CheeseType("Roquefort", 24, Rank.Gold, 475),
            new CheeseType("Emmental", 26, Rank.Platinum, 700),
            new CheeseType("Mascarpone", 28, Rank.Platinum, 800),
            new CheeseType("Bocconcini", 30, Rank.Platinum, 900),
            new CheeseType("Halloumi", 32, Rank.Diamond, 1475),
            new CheeseType("Gorgonzola", 34, Rank.Diamond, 1600),
            new CheeseType("Reblochon", 36, Rank.Diamond, 1725),
            new CheeseType("Oaxaca", 38, Rank.Master, 3050),
            new CheeseType("Adelost", 40, Rank.Master, 3200),
            new CheeseType("Garroxta", 42, Rank.Master, 3350),
            new CheeseType("Juustoleipa", 44, Rank.Grandmaster, 6225),
            new CheeseType("Castelmagno", 46, Rank.Grandmaster, 6400),
            new CheeseType("Myzithra", 48, Rank.Grandmaster, 6575),
            new CheeseType("Beaufort D'Ete", 50, Rank.Legend, 12600),
            new CheeseType("Lord of the Hundreds", 52, Rank.Legend, 12800),
            new CheeseType("Pule", 54, Rank.Legend, 13000),
        };

        public static IReadOnlyList<CheeseVariant> Variants { get; } = new List<CheeseVariant>()
        {
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
