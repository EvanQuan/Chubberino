using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands
{
    internal sealed class CommandRepository
    {
        private IReadOnlyList<ICommand> Commands { get; }

        public CommandRepository(ITwitchClient client, IMessageSpooler spooler)
        {
            Commands = new List<ICommand>()
            {
                new Copy(client, spooler),
                new Greet(client, spooler),
                new Jimbox(client, spooler),
                new Join(client, spooler),
                new Log(client, spooler),
                new MockStreamElements(client, spooler),
                new PogOTh3Gazette(client, spooler),
                new Color(client, spooler),
                new Repeat(client, spooler),
                new Reply(client, spooler),
                new TimeoutAlert(client, spooler),
                new TrackJimbox(client, spooler),
                new TrackPyramids(client, spooler),
                new YepKyle(client, spooler),
            };
        }

        public String GetStatus()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (ICommand command in Commands)
            {
                if (command is Setting setting)
                {
                    stringBuilder.Append(setting.Name + ": " + setting.Status + Environment.NewLine);
                }
            }

            return stringBuilder.ToString();
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
                case "help":
                    Help(arguments.FirstOrDefault());
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
            String property = arguments.FirstOrDefault();

            arguments = arguments.Skip(1);

            if (commandToSet == null)
            {
                Console.WriteLine($"Command \"{commandName}\" not found to set.");
            }
            else if (commandToSet.Set(property, arguments))
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" set to \"{String.Join(" ", arguments)}\"");
            }
            else
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" not set");
            }
        }

        private void Help(String commandName)
        {

            ICommand commandToSet = GetCommand(commandName);

            String message = commandToSet?.GetHelp();

            if (message != null)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine($"Command \"{commandName}\" not found.");
            }
        }

        private ICommand GetCommand(String commandName) => Commands
                .Where(command => command.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
    }
}
