using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public interface ICheeseRepository
    {
        public CheeseType GetRandomType(Int32 cheeseUnlocked);
    }
}
