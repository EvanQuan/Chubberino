﻿using Chubberino.Modules.CheeseGame.Repositories;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class CheeseModifierRepository : Repository<CheeseModifier>
    {
        public static IReadOnlyList<CheeseModifier> Modifiers { get; } = new List<CheeseModifier>()
        {
            null,
            new CheeseModifier("fresh", 1),
            new CheeseModifier("sharp", 2),
            new CheeseModifier("extra-salted", 3),
            new CheeseModifier("smoked", 5),
            new CheeseModifier("extra-creamy", 7),
            new CheeseModifier("aged", 9),
            new CheeseModifier("extra-aged", 12),
            new CheeseModifier("perfect", 15),
        };

        public override IReadOnlyList<CheeseModifier> Values => Modifiers;
    }
}