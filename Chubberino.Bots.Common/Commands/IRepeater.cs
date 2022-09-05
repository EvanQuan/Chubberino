using System;

namespace Chubberino.Bots.Common.Commands;

/// <summary>
/// Repeats an Action at a set interval, asynch.
/// </summary>
[Obsolete("Replace with System.Timers.Timer")]
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
    /// Interval variance for each <see cref="Action"/> repeated.
    /// </summary>
    TimeSpan Variance { get; set; }

    /// <summary>
    /// Action to repeat at the specified <see cref="Interval"/>.
    /// </summary>
    Action Action { get; set; }
}
