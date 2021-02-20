using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;

namespace Chubberino.Client.Commands
{
    public sealed class Refresh : Command
    {
        public IBot Bot { get; }

        public Refresh(ITwitchClientManager client, IConsole console, IBot bot)
            : base(client, console)
        {
            Bot = bot;
        }


        public override void Execute(IEnumerable<String> arguments)
        {
            Bot.Refresh();
        }
    }
}
