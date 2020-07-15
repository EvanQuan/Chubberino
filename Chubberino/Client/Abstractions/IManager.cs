using System;

namespace Chubberino.Client.Abstractions
{
    public interface IManager : IDisposable
    {
        Boolean IsRunning { get; }

        TimeSpan Interval { get; set; }

        Action Task { get; set; }

        void Start();
    }
}
