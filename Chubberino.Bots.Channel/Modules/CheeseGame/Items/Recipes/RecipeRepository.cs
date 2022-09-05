using Chubberino.Database.Models;
using System.Collections.Generic;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Recipes;

public static class RecipeRepository
{
    public static IReadOnlyList<RecipeInfo> Recipes { get; } = new List<RecipeInfo>()
    {
        // Players cannot have an unlock level equal to a value immediately
        // before a negative cheese.
        new RecipeInfo("Babybel", 1),
        new RecipeInfo("Cheddar", 2, Rank.Bronze, 3),
        new RecipeInfo("Mozzarella", 3, Rank.Bronze, 5),
        new RecipeInfo("Swiss", 4, Rank.Bronze, 10),

        // Unlockable cheese types
        new RecipeInfo("Cheshire", 5, Rank.Bronze, 20, true), // 4
        new RecipeInfo("Cheez Wiz", -1),
        new RecipeInfo("Oxford Blue", 6, Rank.Bronze, 40),
        new RecipeInfo("Parmesan", 7, Rank.Bronze, 60),
        new RecipeInfo("Camembert", 8, Rank.Bronze, 0.3),
        new RecipeInfo("Monterey Jack", 9, Rank.Bronze, 0.4),
        new RecipeInfo("Wensleydale", 10, Rank.Bronze, 0.5),
        new RecipeInfo("Red Leicester", 11, Rank.Bronze, 0.55),
        new RecipeInfo("Abertam", 12, Rank.Bronze, 0.6),
        new RecipeInfo("Friulano", 13, Rank.Bronze, 0.65),
        new RecipeInfo("Piave", 14, Rank.Bronze, 0.7),
        new RecipeInfo("Gouda", 15, Rank.Bronze, 0.75),

        new RecipeInfo("Havarti", 16, Rank.Silver, 0.5, true), // 13
        new RecipeInfo("Easy Cheese", -5),
        new RecipeInfo("Tetilla", 17, Rank.Silver, 0.55),
        new RecipeInfo("Morbier", 18, Rank.Silver, 0.6),
        new RecipeInfo("Asiago", 19, Rank.Silver, 0.65),
        new RecipeInfo("Red Cloud", 20, Rank.Silver, 0.7),
        new RecipeInfo("Muenster", 21, Rank.Silver, 0.75),

        new RecipeInfo("Lacey Grey", 22, Rank.Gold, 0.5, true), // 20
        new RecipeInfo("Kraft Singles", -16),
        new RecipeInfo("Grana", 23, Rank.Gold, 0.55),
        new RecipeInfo("Kasseri", 24, Rank.Gold, 0.6),
        new RecipeInfo("Brie", 25, Rank.Gold, 0.65),
        new RecipeInfo("Limburger", 26, Rank.Gold, 0.7),
        new RecipeInfo("Roquefort", 27, Rank.Gold, 0.75),

        new RecipeInfo("Kasseri", 29, Rank.Platinum, 0.5, true), // 27
        new RecipeInfo("Cheese Spread", -22),
        new RecipeInfo("Emmental", 31, Rank.Platinum, 0.55),
        new RecipeInfo("Ogleshield", 33, Rank.Platinum, 0.6),
        new RecipeInfo("Mascarpone", 35, Rank.Platinum, 0.65),
        new RecipeInfo("Sicilian Blend", 37, Rank.Platinum, 0.7),
        new RecipeInfo("Bocconcini", 39, Rank.Platinum, 0.75),

        new RecipeInfo("Neufchatel", 41, Rank.Diamond, 0.5, true), // 34
        new RecipeInfo("String Cheese", -29),
        new RecipeInfo("Halloumi", 43, Rank.Diamond, 0.55),
        new RecipeInfo("Alpine Gold", 45, Rank.Diamond, 0.6),
        new RecipeInfo("Gorgonzola", 47, Rank.Diamond, 0.65),
        new RecipeInfo("Lairobell", 49, Rank.Diamond, 0.7),
        new RecipeInfo("Reblochon", 51, Rank.Diamond, 0.75),

        new RecipeInfo("Edam", 53, Rank.Master, 0.5, true), // 41
        new RecipeInfo("Velveeta", -41),
        new RecipeInfo("Oaxaca", 55, Rank.Master, 0.55),
        new RecipeInfo("Ulloa", 57, Rank.Master, 0.6),
        new RecipeInfo("Adelost", 59, Rank.Master, 0.65),
        new RecipeInfo("Montagnolo", 61, Rank.Master, 0.7),
        new RecipeInfo("Garroxta", 63, Rank.Master, 0.75),

        new RecipeInfo("Cabrales", 66, Rank.Grandmaster, 0.5, true), // 48
        new RecipeInfo("American", -53),
        new RecipeInfo("Juustoleipa", 69, Rank.Grandmaster, 0.55),
        new RecipeInfo("Flagship Block", 72, Rank.Grandmaster, 0.6),
        new RecipeInfo("Castelmagno", 75, Rank.Grandmaster, 0.65),
        new RecipeInfo("Gammelost", 78, Rank.Grandmaster, 0.7),
        new RecipeInfo("Myzithra", 81, Rank.Grandmaster, 0.75),

        new RecipeInfo("Beaufort D'Ete", 84, Rank.Legend, 0.5, true), // 55
        new RecipeInfo("Stinking Bishop", -66),
        new RecipeInfo("Winnimere", 87, Rank.Legend, 0.55),
        new RecipeInfo("Cacio Bufala", 90, Rank.Legend, 0.6),
        new RecipeInfo("Lord of the Hundreds", 93, Rank.Legend, 0.65),
        new RecipeInfo("White Stilton Gold", 96, Rank.Legend, 0.7),
        new RecipeInfo("Pule", 99, Rank.Legend, 0.75),
        new RecipeInfo("Chubmeister", 250, Rank.Legend, 0.8),
    };
}
