using Chubberino.Client.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chubberino.Client.Commands
{
    /// <summary>
    /// Repeats an Action at a set interval, asynch.
    /// </summary>
    public sealed class Repeater : IRepeater
    {
        /// <summary>
        /// Specifies if the repeater has already started.
        /// </summary>
        public Boolean IsRunning { get; private set; }

        /// <summary>
        /// Interval to repeat <see cref="Action"/>.
        /// </summary>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// Action to repeat at the specified <see cref="Interval"/>.
        /// </summary>
        public Action Action { get; set; }

        public Repeater(Action action, TimeSpan interval)
        {
            Action = action;
            Interval = interval;
        }

        /// <summary>
        /// Start executing <see cref="Action"/> at the specified <see cref="Interval"/>.
        /// </summary>
        public void Start()
        {
            // Don't process further if already running.
            if (IsRunning) { return; }

            Task.Run(ExecuteAction);
            IsRunning = true;
        }

        /// <summary>
        /// Stop executing <see cref="Action"/>.
        /// </summary>
        public void Stop()
        {
            // When set to false, ExecuteAction will return, ending the Task.
            IsRunning = false;
        }

        /// <summary>
        /// Continuously run the action at the specified interval.
        /// </summary>
        private void ExecuteAction()
        {
            while (IsRunning)
            {
                Action();
                Thread.Sleep(Interval);
            }
        }
    }
}
