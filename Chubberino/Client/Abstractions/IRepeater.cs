using System;

namespace Chubberino.Client.Abstractions
{
    /// <summary>
    /// Repeats an Action at a set interval, asynch.
    /// </summary>
    public interface IRepeater
    {
        /// <summary>
        /// Specifies if the repeater has already started.
        /// </summary>
        Boolean IsRunning { get; }

        /// <summary>
        /// Interval to repeat <see cref="Action"/>.
        /// </summary>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// Action to repeat at the specified <see cref="Interval"/>.
        /// </summary>
        Action Action { get; set; }

        /// <summary>
        /// Start executing <see cref="Action"/> at the specified <see cref="Interval"/>.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop executing <see cref="Action"/>.
        /// </summary>
        void Stop();
    }
}
