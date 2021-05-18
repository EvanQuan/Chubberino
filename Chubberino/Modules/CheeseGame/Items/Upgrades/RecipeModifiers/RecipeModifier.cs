using System;

namespace Chubberino.Modules.CheeseGame.Items.Upgrades.RecipeModifiers
{
    public sealed class RecipeModifier
    {
        public RecipeModifier(String name, Int32 points)
        {
            Name = name;
            Points = points;
        }

        public String Name { get; }
        public Int32 Points { get; }
    }
}
