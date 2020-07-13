using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.Automation;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    /// <summary>
    /// Repeat a specified message at the throttle limit.
    /// It is not recommended to sent messages manually while messages are
    /// being repeated from this, or you may incur a global IP shadow ban.
    /// </summary>
    internal sealed class Repeat : Setting
    {
        public TimeSpan RepeatInterval { get; set; }
        private String RepeatMessage { get; set; }
        public Repeat(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
        }

        public override String Status => (String.IsNullOrWhiteSpace(RepeatMessage)
            ? "disabled"
            : RepeatMessage)
            + $" Interval: {RepeatInterval.TotalSeconds} seconds";

        public override void Execute(IEnumerable<String> arguments)
        {
            RepeatMessage = String.Join(" ", arguments);
            Spooler.RepeatMessage = RepeatMessage;
        }

        public override Boolean Set(String value, IEnumerable<String> arguments)
        {
            switch (value.ToLower())
            {
                case "i":
                case "interval":
                {
                    if (Double.TryParse(arguments.FirstOrDefault(), out Double result))
                    {
                        RepeatInterval = result >= 0
                            ? TimeSpan.FromSeconds(result)
                            : TimeSpan.Zero;

                        return true;
                    }
                }
                break;
            }
            return false;
        }
    }
}
