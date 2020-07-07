using MouseBot.Implementation.Abstractions;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands.Settings
{
    /// <summary>
    /// Repeat a specified message at the throttle limit.
    /// It is not recommended to sent messages manually while messages are
    /// being repeated from this, or you may incur a global IP shadow ban.
    /// </summary>
    internal sealed class Repeat : Setting
    {
        private String RepeatMessage { get; set; }
        public Repeat(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
        }

        public override String Status => String.IsNullOrWhiteSpace(RepeatMessage)
            ? "disabled"
            : RepeatMessage;

        public override void Execute(IEnumerable<String> arguments)
        {
            RepeatMessage = String.Join(" ", arguments);
            Spooler.RepeatMessage = RepeatMessage;
        }
    }
}
