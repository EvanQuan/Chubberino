using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Chubberino.Client.Commands
{
    public sealed class Refresh : Command
    {
        private IBot Bot { get; }

        public Refresh(IExtendedClient client, TextWriter console, IBot bot)
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
