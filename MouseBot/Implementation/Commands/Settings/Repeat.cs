using MouseBot.Implementation.Abstractions;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands.Settings
{
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
