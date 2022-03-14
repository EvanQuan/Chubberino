using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chubberino.Bots.Common.Commands;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings;

namespace Chubberino.Client.Commands.Settings
{
    /// <summary>
    /// Count from a specified number.
    /// </summary>
    public sealed class Count : Setting
    {
        private IRepeater Repeater { get; }

        private Int32 StartingNumber { get; set; }

        private Int32 CurrentCount { get; set; }

        private String Prefix { get; set; } = String.Empty;

        private String Suffix { get; set; } = String.Empty;

        public override String Status => base.Status
            + $"\n\tstart: {StartingNumber}"
            + $"\n\tcurrent: {CurrentCount}"
            + $"\n\tinterval: {Repeater.Interval.TotalSeconds} seconds"
            + $"\n\tprefix: {Prefix}"
            + $"\n\tsuffix: {Suffix}";

        public Count(ITwitchClientManager client, IRepeater repeater, TextWriter writer)
            : base(client, writer)
        {
            Repeater = repeater;
            Repeater.Action = SpoolCount;
            Repeater.Interval = TimeSpan.FromSeconds(3);
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            base.Execute(arguments);

            Repeater.IsRunning = IsEnabled;
        }

        public override Boolean Set(String property, IEnumerable<String> arguments)
        {
            switch (property?.ToLower())
            {
                case "s":
                case "start":
                    if (Int32.TryParse(arguments.FirstOrDefault(), out Int32 startigNumber))
                    {
                        StartingNumber = startigNumber;
                        // Reset the current count.
                        CurrentCount = StartingNumber;
                        return true;
                    }
                    return false;
                case "i":
                case "interval":
                    if (Double.TryParse(arguments.FirstOrDefault(), out Double seconds))
                    {
                        Repeater.Interval = TimeSpan.FromSeconds(seconds);
                        return true;
                    }
                    return false;
                case "p":
                case "prefix":
                    Prefix = String.Join(" ", arguments);
                    return true;
                case "suffix":
                    Suffix = String.Join(" ", arguments);
                    return true;
                default:
                    return false;
            }
        }

        private void SpoolCount()
        {
            TwitchClientManager.SpoolMessage($"{Prefix} {CurrentCount++} {Suffix}");
        }
    }
}
