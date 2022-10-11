using System.Timers;
using Chubberino.Bots.Common.Commands.Settings.UserCommands;
using RandomSource = System.Random;

namespace Chubberino.Bots.Common.Commands;

/// <summary>
/// Repeats an Action at a set interval, asynch.
/// </summary>
public sealed class Repeater : IRepeater
{
    private RandomSource Random { get; }
    private Timer Timer { get; }

    private Boolean isRunning = false;

    /// <summary>
    /// Specifies if the repeater is currently executing
    /// <see cref="Action"/> at the specified <see cref="Interval"/>.
    /// </summary>
    public Boolean IsRunning
    {
        get => isRunning;
        set
        {
            if (value)
            {
                if (!isRunning)
                {
                    Start();
                }
            }

            isRunning = value;
        }
    }

    /// <summary>
    /// Interval to repeat <see cref="Action"/>.
    /// </summary>
    public TimeSpan Interval { get; set; }

    /// <summary>
    /// Action to repeat at the specified <see cref="Interval"/>.
    /// </summary>
    public Action Action { get; set; }

    public TimeSpan Variance { get; set; } = TimeSpan.Zero;

    public Repeater(RandomSource random)
    {
        Timer = new Timer();
        Timer.Elapsed += Timer_Elapsed;
        Random = random;
    }

    private void Timer_Elapsed(Object sender, ElapsedEventArgs e)
    {
        Action();

        if (HasVariance())
        {
            Timer.Interval = (Interval + GetVariance()).TotalMilliseconds;
        }
    }

    /// <summary>
    /// Start executing <see cref="Action"/> at the specified <see cref="Interval"/>.
    /// </summary>
    private void Start()
        => Timer.Start();

    private TimeSpan GetVariance()
    {
        if (HasVariance())
        {
            return TimeSpan.FromMilliseconds(
                Random.Next(
                    -(Int32)Variance.TotalMilliseconds,
                    (Int32)Variance.TotalMilliseconds));
        }

        return TimeSpan.Zero;
    }

    private Boolean HasVariance()
        => Variance != TimeSpan.Zero;
}
