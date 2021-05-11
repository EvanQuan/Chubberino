using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Repositories
{
    [Obsolete("Replace with IReadOnlyLists")]
    public interface IRepository<TType>
    {
        public IReadOnlyList<TType> Values { get; }

        public Boolean TryGetNextToUnlock(Int32 nextUnlockIndex, out TType nextUnlock);
    }
}
