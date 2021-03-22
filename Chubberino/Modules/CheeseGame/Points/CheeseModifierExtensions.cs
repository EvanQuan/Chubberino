using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public static class CheeseModifierExtensions
    {
        public static CheeseType Modify(this CheeseModifier modifier, CheeseType cheeseType)
        {
            if (modifier == null) { return cheeseType; }
            return new CheeseType(modifier.Name + " " + cheeseType.Name, Math.Sign(cheeseType.PointValue) * modifier.Points + cheeseType.PointValue);
        }
    }
}
