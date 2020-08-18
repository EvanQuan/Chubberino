using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands
{
    public sealed class DisableAll : Command
    {
        public IEnumerable<ICommand> Commands { get; }

        public DisableAll(ITwitchClient client, IMessageSpooler spooler, IEnumerable<ICommand> commands)
            : base(client, spooler)
        {
            Commands = commands;
        }


        public override void Execute(IEnumerable<String> arguments)
        {
            foreach (ICommand command in Commands)
            {
                if (command is ISetting setting)
                {
                    setting.IsEnabled = false;
                }
            }

            Console.WriteLine("Disabled all settings");
        }
    }
}
