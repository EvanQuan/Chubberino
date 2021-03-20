using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public interface ICheeseRepository
    {
        public Boolean TryGetNextCheeseToUnlock(Player player, out CheeseType cheeseType);

        public CheeseType GetRandomType(Int32 cheeseUnlocked);
    }
}
