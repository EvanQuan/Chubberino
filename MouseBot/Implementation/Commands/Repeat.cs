using MouseBot.Implementation.Abstractions;
using System;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands
{
    public sealed class Repeat : Command
    {
        public Repeat(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
        }

        public override void Execute(params String[] arguments)
        {
            Spooler.RepeatMessage = arguments.Length >= 1 ? String.Join(" ", arguments) : null;
        }
    }
}
