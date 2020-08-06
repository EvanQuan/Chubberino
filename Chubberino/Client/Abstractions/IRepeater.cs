using System;

namespace Chubberino.Client.Abstractions
{
    /// <summary>
    /// Repeats an Action at a set interval, asynch.
    /// </summary>
    public interface IRepeater
    {
        /// <summary>
        /// Specifies if the repeater is currently executing
        /// <see cref="Action"/> at the specified <see cref="Interval"/>.
        /// </summary>
        Boolean IsRunning { get; set; }

        /// <summary>
        /// Interval to repeat <see cref="Action"/>.
        /// </summary>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// Action to repeat at the specified <see cref="Interval"/>.
        /// </summary>
        Action Action { get; set; }
    }
}
