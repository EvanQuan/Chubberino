using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame
{
    public interface IRepository<TType>
    {
        public Boolean TryGetNextToUnlock(Player player, out TType nextUnlock);

        public TType GetRandom(Int32 unlocked);
    }
}
