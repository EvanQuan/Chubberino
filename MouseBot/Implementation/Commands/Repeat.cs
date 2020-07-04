using MouseBot.Implementation.Abstractions;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands
{
    public sealed class Repeat : Command
    {
        public Repeat(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            Spooler.RepeatMessage = String.Join(" ", arguments);
        }
    }
}
