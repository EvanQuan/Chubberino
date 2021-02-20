using System;
using System.Collections.Generic;

namespace Chubberino.Client.Commands
{
    public sealed class Say : Command
    {
        public Say(ITwitchClientManager client, IConsole console)
            : base(client, console)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            TwitchClientManager.SpoolMessage(String.Join(" ", arguments));
        }
    }
}
