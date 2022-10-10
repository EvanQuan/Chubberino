using System.Threading;

namespace Chubberino.Common.Services;

public sealed class SpinWaitService : ISpinWaitService
{
    public Boolean SpinUntil(Func<Boolean> condition, TimeSpan timeout)
    {
        return SpinWait.SpinUntil(condition, timeout);
    }
}
