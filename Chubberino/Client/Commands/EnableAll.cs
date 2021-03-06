﻿using System.Collections.Generic;

namespace Chubberino.Client.Commands
{
    public sealed class EnableAll : Command
    {
        public ICommandRepository Commands { get; }

        public EnableAll(ITwitchClientManager client, ICommandRepository commands, IConsole console)
            : base(client, console)
        {
            Commands = commands;
        }

        public override void Execute(IEnumerable<string> arguments)
        {
            Commands.EnableAllUserCommands();
            Console.WriteLine("Enabled all user commands");
        }

        public override string GetHelp()
        {
            return @"
Enables all user commmands.

usage: enableall
";
        }

    }
}
