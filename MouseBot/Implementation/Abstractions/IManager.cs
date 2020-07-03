using System;
using System.Collections.Generic;
using System.Text;

namespace MouseBot.Implementation.Abstractions
{
    public interface IManager : IDisposable
    {
        Boolean IsRunning { get; }

        void Start();
    }
}
