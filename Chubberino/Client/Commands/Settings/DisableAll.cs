using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class DisableAll : Command
    {
        private CommandRepository Commands { get; }

        public DisableAll(IExtendedClient client, CommandRepository commands, TextWriter console)
            : base(client, console)
        {
            Commands = commands;
        }

        public override void Execute(IEnumerable<String> arguments)
        {

        }
    }
}
