using System;

namespace Chubberino.Client.Abstractions
{
    public interface IManager : IDisposable
    {
        Boolean IsRunning { get; }

        void Start();
    }
}
