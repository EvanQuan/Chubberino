using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chubberino.Client.Commands
{
    public sealed class DisableAll : Command
    {
        public ICommandRepository Commands { get; }

        public DisableAll(ITwitchClientManager client, ICommandRepository commands, IConsole console)
            : base(client, console)
        {
            Commands = commands;
        }


        public override void Execute(IEnumerable<String> arguments)
        {
            if ('u' == (arguments.FirstOrDefault()?[0] ?? default))
            {
                Commands.DisableAllUserCommands();
                Console.WriteLine("Disabled all user commands.");
            }
            else
            {
                Commands.DisableAllSettings();
                Console.WriteLine("Disabled all settings.");
            }

        }

        public override String GetHelp()
        {
            return @"
Disables all settings.

usage: disableall [type]

    [type]  default - All settings
            u - All user commands
";
        }
    }
}
