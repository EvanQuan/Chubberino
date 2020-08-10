﻿using Chubberino.Client.Abstractions;
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
        private Random Random { get; set; } = new Random();

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
                isRunning = value;

                if (isRunning)
                {
                    Start();
                }
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

        public TimeSpan Variance { get; set; } = TimeSpan.FromSeconds(0.3);

        /// <summary>
        /// Start executing <see cref="Action"/> at the specified <see cref="Interval"/>.
        /// </summary>
        private void Start()
        {
            Task.Run(ExecuteAction);
        }

        /// <summary>
        /// Continuously run the action at the specified interval.
        /// </summary>
        private void ExecuteAction()
        {
            while (isRunning)
            {
                Action();
                Thread.Sleep(Interval + GetVariance());
            }
        }

        private TimeSpan GetVariance()
        {
            return TimeSpan.FromMilliseconds(Random.Next(-(Int32)Variance.TotalMilliseconds, (Int32)Variance.TotalMilliseconds));
        }
    }
}
