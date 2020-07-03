using MouseBot.Implementation.Abstractions;
using MouseBot.Implementation.Commands.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands
{
    internal sealed class CommandRepository
    {
        private IReadOnlyList<ICommand> Commands { get; }

        public CommandRepository(ITwitchClient client, IMessageSpooler spooler)
        {
            Commands = new List<ICommand>()
            {
                new Greet(client, spooler),
                new Interval(client, spooler),
                new Join(client, spooler),
                new Log(client, spooler),
                new Mirror(client, spooler),
                new RandomColors(client, spooler),
                new Repeat(client, spooler),
            };
        }

        public void Execute(String commandName, params String[] arguments)
        {
            if (String.IsNullOrWhiteSpace(commandName)) { return; }

            ICommand commandToExecute = Commands
                .Where(command => command.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (commandToExecute == null)
            {
                Console.WriteLine($"Command \"{commandName}\" not found.");
            }
            else
            {
                commandToExecute.Execute(arguments);
            }
        }
    }
}
