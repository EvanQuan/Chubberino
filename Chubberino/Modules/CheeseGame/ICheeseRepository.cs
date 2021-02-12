using System;

namespace Chubberino.Modules.CheeseGame
{
    public interface ICheeseRepository
    {
        public CheeseType GetRandomType(Int32 cheeseUnlocked);
    }
}
