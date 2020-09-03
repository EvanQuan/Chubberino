using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;

namespace Chubberino.Client.Commands
{
    public sealed class DisableAll : Command
    {
        public CommandRepository Commands { get; }

        public DisableAll(IExtendedClient client, CommandRepository commands)
            : base(client)
        {
            Commands = commands;
        }


        public override void Execute(IEnumerable<String> arguments)
        {
            Commands.DisableAllSettings();

            Console.WriteLine("Disabled all settings");
        }
    }
}
