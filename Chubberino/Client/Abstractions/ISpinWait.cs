using System;

namespace Chubberino.Client.Abstractions
{
    /// <summary>
    /// Provides support for spin-based waiting.
    /// </summary>
    public interface ISpinWait
    {
        /// <summary>
        ///  Spins until the specified condition is satisfied or until the
        ///  specified timeout is expired.
        /// </summary>
        /// <param name="condition">
        /// A delegate to be executed over and over until it returns true.
        /// </param>
        /// <param name="timeout">
        /// A <see cref="TimeSpan"/> that represents the number of milliseconds
        /// to wait, or a TimeSpan that represents -1 milliseconds to wait
        /// indefinitely.
        /// </param>
        /// <returns>
        /// true if the condition is satisfied within the timeout; otherwise,
        /// false
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The condition argument is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// timeout is a negative number other than -1 milliseconds, which represents an
        /// infinite time-out -or- timeout is greater than System.Int32.MaxValue.
        /// </exception>
        Boolean SpinUntil(Func<Boolean> condition, TimeSpan timeout);

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
}
