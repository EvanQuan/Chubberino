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
            new CheeseType("Easy Cheese", 1),
            new CheeseType("Cheez Wiz", 2, Rank.Bronze, 3),
            new CheeseType("Velveeta", 3, Rank.Bronze, 5),
            new CheeseType("American", 4, Rank.Bronze, 10),

            // Unlockable cheese types
            new CheeseType("Cheddar", 5, Rank.Bronze, 20, true), // 4
            new CheeseType("Blue Castello", -1),
            new CheeseType("Mozzarella", 6, Rank.Bronze, 40),
            new CheeseType("Friulano", 7, Rank.Bronze, 60),
            new CheeseType("Swiss", 8, Rank.Bronze, 80),
            new CheeseType("Tetilla", 9, Rank.Bronze, 90),
            new CheeseType("Babybel", 10, Rank.Bronze, 100),
            new CheeseType("Piave", 11, Rank.Bronze, 110),
            new CheeseType("Gouda", 12, Rank.Bronze, 120),

            new CheeseType("Havarti", 13, Rank.Silver, 140, true), // 13
            new CheeseType("Stinking Bishop", -5),
            new CheeseType("Monterey Jack", 14, Rank.Silver, 160),
            new CheeseType("Morbier", 15, Rank.Silver, 180),
            new CheeseType("Asiago", 16, Rank.Silver, 200),
            new CheeseType("Red Cloud", 17, Rank.Silver, 220),
            new CheeseType("Camembert", 18, Rank.Silver, 240),

            new CheeseType("Lacey Grey", 19, Rank.Gold, 280, true), // 20
            new CheeseType("Cheshire", -13),
            new CheeseType("Grana", 20, Rank.Gold, 320),
            new CheeseType("Kasseri", 21, Rank.Gold, 360),
            new CheeseType("Brie", 22, Rank.Gold, 400),
            new CheeseType("Parmesan", 23, Rank.Gold, 440),
            new CheeseType("Roquefort", 24, Rank.Gold, 480),

            new CheeseType("Kasseri", 25, Rank.Platinum, 560, true), // 27
            new CheeseType("Limburger", -19),
            new CheeseType("Emmental", 26, Rank.Platinum, 640),
            new CheeseType("Ogleshield", 27, Rank.Platinum, 720),
            new CheeseType("Mascarpone", 28, Rank.Platinum, 800),
            new CheeseType("Wensleydale", 29, Rank.Platinum, 880),
            new CheeseType("Bocconcini", 30, Rank.Platinum, 960),

            new CheeseType("Neufchatel", 31, Rank.Diamond, 1120, true), // 34
            new CheeseType("Oxford Blue", -25),
            new CheeseType("Halloumi", 32, Rank.Diamond, 1280),
            new CheeseType("Alpine Gold", 33, Rank.Diamond, 1440),
            new CheeseType("Gorgonzola", 34, Rank.Diamond, 1600),
            new CheeseType("Lairobell", 35, Rank.Diamond, 1760),
            new CheeseType("Reblochon", 36, Rank.Diamond, 1920),

            new CheeseType("Edam", 37, Rank.Master, 2240, true), // 41
            new CheeseType("Muenster", -31),
            new CheeseType("Oaxaca", 38, Rank.Master, 2560),
            new CheeseType("Ulloa", 39, Rank.Master, 2880),
            new CheeseType("Adelost", 40, Rank.Master, 3200),
            new CheeseType("Montagnolo", 41, Rank.Master, 3520),
            new CheeseType("Garroxta", 42, Rank.Master, 3840),

            new CheeseType("Cabrales", 43, Rank.Grandmaster, 4480, true), // 48
            new CheeseType("Danablu", -37),
            new CheeseType("Juustoleipa", 44, Rank.Grandmaster, 5120),
            new CheeseType("Flagship Block", 45, Rank.Grandmaster, 5760),
            new CheeseType("Castelmagno", 46, Rank.Grandmaster, 6400),
            new CheeseType("Gammelost", 47, Rank.Grandmaster, 7040),
            new CheeseType("Myzithra", 48, Rank.Grandmaster, 7680),

            new CheeseType("Beaufort D'Ete", 49, Rank.Legend, 8960, true), // 55
            new CheeseType("Epoisses", -43),
            new CheeseType("Winnimere", 50, Rank.Legend, 10240),
            new CheeseType("Cacio Bufala", 51, Rank.Legend, 11520),
            new CheeseType("Lord of the Hundreds", 52, Rank.Legend, 12800),
            new CheeseType("White Stilton Gold", 53, Rank.Legend, 14080),
            new CheeseType("Pule", 54, Rank.Legend, 15360),
            new CheeseType("Chubmeister", 100, Rank.Legend, 16000),
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
