using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Utility;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class CheeseRepository : IRepository<CheeseType>
    {
        public static IReadOnlyList<CheeseType> Cheeses { get; } = new List<CheeseType>()
        {
            // Players cannot have an unlock level equal to a value immediately
            // before a negative cheese.
            new CheeseType("Babybel", 1),
            new CheeseType("Cheddar", 2, Rank.Bronze, 3),
            new CheeseType("Mozzarella", 3, Rank.Bronze, 5),
            new CheeseType("Swiss", 4, Rank.Bronze, 10),

            // Unlockable cheese types
            new CheeseType("Cheshire", 5, Rank.Bronze, 20, true), // 4
            new CheeseType("Cheez Wiz", -1),
            new CheeseType("Oxford Blue", 6, Rank.Bronze, 40),
            new CheeseType("Parmesan", 7, Rank.Bronze, 60),
            new CheeseType("Camembert", 8, Rank.Bronze, 0.3),
            new CheeseType("Monterey Jack", 9, Rank.Bronze, 0.4),
            new CheeseType("Wensleydale", 10, Rank.Bronze, 0.5),
            new CheeseType("Red Leicester", 11, Rank.Bronze, 0.55),
            new CheeseType("Abertam", 12, Rank.Bronze, 0.6),
            new CheeseType("Friulano", 13, Rank.Bronze, 0.65),
            new CheeseType("Piave", 14, Rank.Bronze, 0.7),
            new CheeseType("Gouda", 15, Rank.Bronze, 0.75),

            new CheeseType("Havarti", 16, Rank.Silver, 0.5, true), // 13
            new CheeseType("Easy Cheese", -5),
            new CheeseType("Tetilla", 17, Rank.Silver, 0.55),
            new CheeseType("Morbier", 18, Rank.Silver, 0.6),
            new CheeseType("Asiago", 19, Rank.Silver, 0.65),
            new CheeseType("Red Cloud", 20, Rank.Silver, 0.7),
            new CheeseType("Muenster", 21, Rank.Silver, 0.75),

            new CheeseType("Lacey Grey", 22, Rank.Gold, 0.5, true), // 20
            new CheeseType("Kraft Singles", -16),
            new CheeseType("Grana", 23, Rank.Gold, 0.55),
            new CheeseType("Kasseri", 24, Rank.Gold, 0.6),
            new CheeseType("Brie", 25, Rank.Gold, 0.65),
            new CheeseType("Limburger", 26, Rank.Gold, 0.7),
            new CheeseType("Roquefort", 27, Rank.Gold, 0.75),

            new CheeseType("Kasseri", 29, Rank.Platinum, 0.5, true), // 27
            new CheeseType("Cheese Spread", -22),
            new CheeseType("Emmental", 31, Rank.Platinum, 0.55),
            new CheeseType("Ogleshield", 33, Rank.Platinum, 0.6),
            new CheeseType("Mascarpone", 35, Rank.Platinum, 0.65),
            new CheeseType("Sicilian Blend", 37, Rank.Platinum, 0.7),
            new CheeseType("Bocconcini", 39, Rank.Platinum, 0.75),

            new CheeseType("Neufchatel", 41, Rank.Diamond, 0.5, true), // 34
            new CheeseType("String Cheese", -29),
            new CheeseType("Halloumi", 43, Rank.Diamond, 0.55),
            new CheeseType("Alpine Gold", 45, Rank.Diamond, 0.6),
            new CheeseType("Gorgonzola", 47, Rank.Diamond, 0.65),
            new CheeseType("Lairobell", 49, Rank.Diamond, 0.7),
            new CheeseType("Reblochon", 51, Rank.Diamond, 0.75),

            new CheeseType("Edam", 53, Rank.Master, 0.5, true), // 41
            new CheeseType("Velveeta", -41),
            new CheeseType("Oaxaca", 55, Rank.Master, 0.55),
            new CheeseType("Ulloa", 57, Rank.Master, 0.6),
            new CheeseType("Adelost", 59, Rank.Master, 0.65),
            new CheeseType("Montagnolo", 61, Rank.Master, 0.7),
            new CheeseType("Garroxta", 63, Rank.Master, 0.75),

            new CheeseType("Cabrales", 66, Rank.Grandmaster, 0.5, true), // 48
            new CheeseType("American", -53),
            new CheeseType("Juustoleipa", 69, Rank.Grandmaster, 0.55),
            new CheeseType("Flagship Block", 72, Rank.Grandmaster, 0.6),
            new CheeseType("Castelmagno", 75, Rank.Grandmaster, 0.65),
            new CheeseType("Gammelost", 78, Rank.Grandmaster, 0.7),
            new CheeseType("Myzithra", 81, Rank.Grandmaster, 0.75),

            new CheeseType("Beaufort D'Ete", 84, Rank.Legend, 0.5, true), // 55
            new CheeseType("Stinking Bishop", -66),
            new CheeseType("Winnimere", 87, Rank.Legend, 0.55),
            new CheeseType("Cacio Bufala", 90, Rank.Legend, 0.6),
            new CheeseType("Lord of the Hundreds", 93, Rank.Legend, 0.65),
            new CheeseType("White Stilton Gold", 96, Rank.Legend, 0.7),
            new CheeseType("Pule", 99, Rank.Legend, 0.75),
            new CheeseType("Chubmeister", 250, Rank.Legend, 0.8),
        };

        public Random Random { get; }

        public CheeseRepository(Random random)
        {
            Random = random;
        }

        public CheeseType GetRandom(Int32 unlocked)
        {
            return Cheeses[Random.Next(unlocked.Min(Cheeses.Count - 1).Max(0))];
        }

        public Boolean TryGetNextToUnlock(Player player, out CheeseType nextUnlock)
        {
            Int32 nextCheese = player.CheeseUnlocked + 1;
            if (nextCheese >= Cheeses.Count)
            {
                nextUnlock = default;
                return false;
            }

            nextUnlock = Cheeses[nextCheese];
            return true;
        }
    }
}
