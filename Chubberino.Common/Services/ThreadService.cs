using System;
using System.Threading;

namespace Chubberino.Common.Services
{
    public sealed class ThreadService : IThreadService
    {
        public void Sleep(TimeSpan timeout)
        {
            Thread.Sleep(timeout);
        }

    }
}
