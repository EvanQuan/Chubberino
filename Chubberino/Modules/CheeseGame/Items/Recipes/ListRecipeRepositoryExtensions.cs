﻿using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Utility;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Items.Recipes
{
    public static class ListRecipeRepositoryExtensions
    {
        public static Boolean TryGetNextToUnlock(
            this IReadOnlyList<RecipeInfo> repository,
            Player player,
            out RecipeInfo cheeseType)
        {
            // Since the player starts off with 1 cheese unlocked by default,
            // we always can return the 0th element of the repository.
            // We add 1 to ensure this.
            return repository.TryGet(player.CheeseUnlocked + 1, out cheeseType);
        }
    }
}
