using Chubberino.Utility;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Repositories
{
    public abstract class Repository<TType> : IRepository<TType>
    {
        public abstract IReadOnlyList<TType> Values { get; }

        public Boolean TryGetNextToUnlock(Int32 nextUnlockIndex, out TType nextUnlock)
        {
            return Values.TryGet(nextUnlockIndex, out nextUnlock);
        }
    }
}
