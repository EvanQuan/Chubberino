using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public interface ICheeseRepository
    {
        public CheeseType GetNextCheeseToUnlock(Player player);

        public CheeseType GetRandomType(Int32 cheeseUnlocked);
    }
}
