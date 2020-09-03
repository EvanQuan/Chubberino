using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;

namespace Chubberino.Client.Commands
{
    public sealed class DisableAll : Command
    {
        public ICommandRepository Commands { get; }

        public DisableAll(IExtendedClient client, ICommandRepository commands)
            : base(client)
        {
            Commands = commands;
        }


        public override void Execute(IEnumerable<String> arguments)
        {
            Commands.DisableAllSettings();

            Console.WriteLine("Disabled all settings");
        }

        public override String GetHelp()
        {
            return "Disables all settings.";
        }
    }
}
