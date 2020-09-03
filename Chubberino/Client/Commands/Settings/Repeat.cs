using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Client.Commands.Settings
{
    /// <summary>
    /// Repeat a specified message at the throttle limit.
    /// It is not recommended to sent messages manually while messages are
    /// being repeated from this, or you may incur a global IP shadow ban.
    /// </summary>
    public sealed class Repeat : Setting
    {
        private String RepeatMessage { get; set; }

        private IRepeater Repeater { get; }

        public Repeat(IExtendedClient client, IRepeater repeater)
            : base(client)
        {
            Repeater = repeater;
            Repeater.Action = SpoolRepeatMessages;
            Repeater.Interval = TimeSpan.FromSeconds(0.3);
        }

        private void SpoolRepeatMessages()
        {
            TwitchClient.SpoolMessage(RepeatMessage);
        }

        public override String Status => base.Status
            + $"\n\tMessage: {RepeatMessage}"
            + $"\n\tInterval: {Repeater.Interval.TotalSeconds} seconds"
            + $"\n\tVariance: {Repeater.Variance.TotalSeconds} seconds";

        public override void Execute(IEnumerable<String> arguments)
        {
            String proposedRepeatMessage = String.Join(" ", arguments);

            if (String.IsNullOrEmpty(proposedRepeatMessage))
            {
                // No arguments toggles.
                IsEnabled = !IsEnabled;
            }
            else
            {
                // Update the message and keep repeating.
                RepeatMessage = proposedRepeatMessage;
                IsEnabled = true;
            }

            Repeater.IsRunning = IsEnabled;
        }

        public override Boolean Set(String property, IEnumerable<String> arguments)
        {
            switch (property?.ToLower())
            {
                case "m":
                case "message":
                    RepeatMessage = String.Join(" ", arguments);
                    return true;
                case "i":
                case "interval":
                {
                    if (Double.TryParse(arguments.FirstOrDefault(), out Double result))
                    {
                        Repeater.Interval = result >= 0
                            ? TimeSpan.FromSeconds(result)
                            : TimeSpan.Zero;

                        return true;
                    }
                }
                break;
                case "v":
                case "variance":
                {
                    if (Double.TryParse(arguments.FirstOrDefault(), out Double result))
                    {
                        Repeater.Variance = result >= 0
                            ? TimeSpan.FromSeconds(result)
                            : TimeSpan.Zero;

                        return true;
                    }
                }
                break;
            }
            return false;
        }

        public override String GetHelp()
        {
            return @"
It is recommended to not go below 1.5 seconds or type any other messages
manually, or have other settings enabled for short intervals  to avoid a global
shadow ban.
";
        }
    }
}
