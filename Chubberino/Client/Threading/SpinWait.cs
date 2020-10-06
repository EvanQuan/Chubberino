using Chubberino.Client.Abstractions;
using System;

namespace Chubberino.Client.Threading
{
    public sealed class SpinWait : ISpinWait
    {
        public Boolean SpinUntil(Func<Boolean> condition, TimeSpan timeout)
        {
            return System.Threading.SpinWait.SpinUntil(condition, timeout);
        }
    }
}
