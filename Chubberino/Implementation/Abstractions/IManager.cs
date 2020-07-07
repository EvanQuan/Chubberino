using System;

namespace Chubberino.Implementation.Abstractions
{
    public interface IManager : IDisposable
    {
        Boolean IsRunning { get; }

        void Start();
    }
}
