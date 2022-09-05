using System;

namespace Chubberino.Common.Services;

public interface IThreadService
{
    /// <summary>
    /// Suspends the current thread for the specified amount of time.
    /// </summary>
    /// <param name="timeout">
    /// The amount of time for which the thread is suspended. If the value
    /// of the <paramref name="timeout"/> argument is
    /// <see cref="TimeSpan.Zero"/>, the thread relinquishes the remainder
    /// of its time slice to any thread of equal priority that is ready to
    /// run. If there are no other threads of equal priority that are ready
    /// to run, execution of the current thread is not suspended.
    /// </param>
    void Sleep(TimeSpan timeout);
}
