﻿using System;

namespace Chubberino.Client.Threading
{
    public sealed class SpinWait : ISpinWait
    {
        public void Sleep(TimeSpan timeout)
        {
            System.Threading.Thread.Sleep(timeout);
        }

        public Boolean SpinUntil(Func<Boolean> condition, TimeSpan timeout)
        {
            return System.Threading.SpinWait.SpinUntil(condition, timeout);
        }
    }
}
