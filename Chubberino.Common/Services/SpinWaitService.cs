using System;

namespace Chubberino.Common.Services
{
    public sealed class SpinWaitService : ISpinWaitService
    {
        public Boolean SpinUntil(Func<Boolean> condition, TimeSpan timeout)
        {
            return System.Threading.SpinWait.SpinUntil(condition, timeout);
        }
    }
}
