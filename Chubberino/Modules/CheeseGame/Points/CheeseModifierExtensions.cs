using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public static class CheeseModifierExtensions
    {
        public static CheeseType Modify(this CheeseModifier modifier, CheeseType cheeseType)
        {
            if (modifier == null) { return cheeseType; }
            return new CheeseType(modifier.Name + " " + cheeseType.Name, Math.Sign(cheeseType.Points) * modifier.Points + cheeseType.Points);
        }

        public static Int32 GetPoints(this CheeseModifier modifier, Player player)
        {
            return player.GetModifiedPoints(modifier.Points);
        }
    }
}
