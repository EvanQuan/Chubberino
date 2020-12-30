using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chubberino.Client.Commands
{
    public sealed class CommandRepository : ICommandRepository
    {
        public IReadOnlyList<ICommand> Commands => CommandList;

        private List<ICommand> CommandList { get; }

        private TextWriter Console { get; }

        private Lazy<IEnumerable<ISetting>> LazySettings { get; }

        private Lazy<IEnumerable<IUserCommand>> LazyUserCommands { get; }

        public CommandRepository(TextWriter console)
        {
            Console = console;
            CommandList = new List<ICommand>();
            LazySettings = new Lazy<IEnumerable<ISetting>>(() =>
            {
                var settingList = new List<ISetting>();

                foreach (ICommand command in CommandList)
                {
                    if (command is ISetting setting)
                    {
                        settingList.Add(setting);
                    }
                }

                return settingList;
            });

            LazyUserCommands = new Lazy<IEnumerable<IUserCommand>>(() =>
            {
                var userCommandList = new List<IUserCommand>();

                foreach (ISetting setting in Settings)
                {
                    if (setting is IUserCommand userCommand)
                    {
                        userCommandList.Add(userCommand);
                    }
                }

                return userCommandList;
            });
        }

        public ICommandRepository AddCommand(ICommand command)
        {
            CommandList.Add(command);
            return this;
        }

        /// <summary>
        /// Sets <see cref="ISetting.IsEnabled"/> to false for all settings.
        /// </summary>
        public void DisableAllSettings()
        {
            foreach (ISetting setting in Settings)
            {
                setting.IsEnabled = false;
            }
        }

        public void RefreshAll(IExtendedClient twitchClient)
        {
            foreach (ICommand command in CommandList)
            {
                command.Refresh(twitchClient);
            }
        }

        public String GetStatus()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (ICommand command in CommandList)
            {
                if (command is ISetting setting)
                {
                    stringBuilder.Append(setting.Name + ": " + setting.Status + Environment.NewLine);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get all the <see cref="ISetting"/>s contained within <see cref="CommandList"/>.
        /// </summary>
        /// <returns>all the <see cref="ISetting"/>s contained within <see cref="CommandList"/>.</returns>
        public IEnumerable<ISetting> Settings => LazySettings.Value;

        /// <summary>
        /// Get all the <see cref="IUserCommand"/>s contained within <see cref="CommandList"/>.
        /// </summary>
        /// <returns>all the <see cref="IUserCommand"/>s contained within <see cref="CommandList"/>.</returns>
        public IEnumerable<IUserCommand> UserCommands => LazyUserCommands.Value;

        public void Execute(String commandName, IEnumerable<String> arguments)
        {
            if (String.IsNullOrWhiteSpace(commandName)) { return; }

            switch (commandName)
            {
                // Meta commands
                case "g":
                case "get":
                    Get(arguments.FirstOrDefault(), arguments.Skip(1));
                    break;
                case "s":
                case "set":
                    Set(arguments.FirstOrDefault(), arguments.Skip(1));
                    break;
                case "a":
                case "add":
                    Add(arguments.FirstOrDefault(), arguments.Skip(1));
                    break;
                case "r":
                case "remove":
                    Remove(arguments.FirstOrDefault(), arguments.Skip(1));
                    break;
                case "h":
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
                    Console.WriteLine($"Command \"{commandName}\" value \"{String.Join(" ", arguments)}\" not found.");
                }
                else
                {
                    Console.WriteLine($"Command \"{commandName}\" value \"{String.Join(" ", arguments)}\" is \"{value}\".");
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
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" set to \"{String.Join(" ", arguments)}\".");
            }
            else
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" not set.");
            }
        }

        private void Add(String commandName, IEnumerable<String> arguments)
        {
            ICommand commandToAddTo = GetCommand(commandName);
            String property = arguments.FirstOrDefault();

            arguments = arguments.Skip(1);

            if (commandToAddTo == null)
            {
                Console.WriteLine($"Command \"{commandName}\" not found to add to.");
            }
            else if (commandToAddTo.Add(property, arguments))
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" added \"{String.Join(" ", arguments)}\".");
            }
            else
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" not added to.");
            }
        }

        private void Remove(String commandName, IEnumerable<String> arguments)
        {
            ICommand commandToRemoveFrom = GetCommand(commandName);
            String property = arguments.FirstOrDefault();

            arguments = arguments.Skip(1);

            if (commandToRemoveFrom == null)
            {
                Console.WriteLine($"Command \"{commandName}\" not found to add remove from.");
            }
            else if (commandToRemoveFrom.Remove(property, arguments))
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" removed \"{String.Join(" ", arguments)}\".");
            }
            else
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" not removed from.");
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

        private ICommand GetCommand(String commandName) => CommandList
                .Where(command => command.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
    }
}
