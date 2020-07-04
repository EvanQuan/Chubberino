﻿using MouseBot.Implementation.Abstractions;
using MouseBot.Implementation.Commands.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
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
                new Copy(client, spooler),
                new RandomColors(client, spooler),
                new Repeat(client, spooler),
            };
        }

        public void Execute(String commandName, IEnumerable<String> arguments)
        {
            if (String.IsNullOrWhiteSpace(commandName)) { return; }

            switch (commandName)
            {
                // Meta commands
                case "get":
                    Get(arguments.FirstOrDefault(), arguments.Skip(1));
                    break;
                case "set":
                    Set(arguments.FirstOrDefault(), arguments.Skip(1));
                    break;

                // Regular commands
                default:
                    ICommand commandToExecute = GetCommand(commandName);

                    if (commandToExecute == null)
                    {
                        Console.WriteLine($"Command \"{commandName}\" not found.");
                    }
                    else
                    {
                        commandToExecute.Execute(arguments);
                    }
                    break;
            }

        }

        private void Get(String commandName, IEnumerable<String> arguments)
        {
            ICommand commandToSet = GetCommand(commandName);

            if (commandToSet == null)
            {
                Console.WriteLine($"Command \"{commandName}\" not found to get.");
            }
            else
            {
                String value = commandToSet.Get(arguments);
                if (String.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine($"Command \"{commandName}\" value \"{String.Join(" ", arguments)}\" not found");
                }
                else
                {
                    Console.WriteLine($"Command \"{commandName}\" value \"{String.Join(" ", arguments)}\" is \"{value}\"");
                }
            }
        }

        private void Set(String commandName, IEnumerable<String> arguments)
        {
            ICommand commandToSet = GetCommand(commandName);
            String value = arguments.FirstOrDefault();

            arguments = arguments.Skip(1);

            if (commandToSet == null)
            {
                Console.WriteLine($"Command \"{commandName}\" not found to set.");
            }
            else if (commandToSet.Set(value, arguments))
            {
                Console.WriteLine($"Command \"{commandName}\" value \"{value}\" set to \"{String.Join(" ", arguments)}\"");
            }
            else
            {
                Console.WriteLine($"Command \"{commandName}\" value \"{value}\" not set");
            }
        }

        private ICommand GetCommand(String commandName) => Commands
                .Where(command => command.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
    }
}
