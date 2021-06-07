using Chubberino.Client.Commands.Settings;
using Chubberino.Client.Commands.Settings.UserCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chubberino.Client.Commands
{
    public sealed class CommandRepository : ICommandRepository
    {
        private Dictionary<String, ICommand> Commands { get; }

        private IConsole Console { get; }

        private Lazy<IEnumerable<ISetting>> LazySettings { get; }

        private Lazy<IEnumerable<IUserCommand>> LazyUserCommands { get; }

        public CommandRepository(IConsole console)
        {
            Console = console;
            Commands = new();
            LazySettings = new Lazy<IEnumerable<ISetting>>(() =>
            {
                var settingList = new List<ISetting>();

                foreach (ISetting setting in Commands.Values.Where(x => x is ISetting))
                {
                    settingList.Add(setting);
                }

                return settingList;
            });

            LazyUserCommands = new Lazy<IEnumerable<IUserCommand>>(() =>
            {
                var userCommandList = new List<IUserCommand>();

                foreach (IUserCommand userCommand in Settings.Where(x => x is IUserCommand))
                {
                    userCommandList.Add(userCommand);
                }

                return userCommandList;
            });
        }

        public ICommandRepository AddCommand(ICommand command)
        {
            Commands.Add(command.Name, command);
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

        public void DisableAllUserCommands()
        {
            foreach (IUserCommand command in UserCommands)
            {
                command.IsEnabled = false;
            }
        }

        public void EnableAllUserCommands()
        {
            foreach (IUserCommand command in UserCommands)
            {
                command.IsEnabled = true;
            }
        }

        public void RefreshAll()
        {
            foreach (ISetting setting in Settings)
            {
                setting.Refresh();
            }
        }

        public String GetStatus()
        {
            StringBuilder stringBuilder = new();

            // Disabled settings are first.
            var disabledSettingsFirst = Settings
                .OrderBy(x => x.IsEnabled);

            var enabledSettings = disabledSettingsFirst
                .SkipWhile(setting =>
                {
                    if (setting.IsEnabled) { return false; }

                    stringBuilder.AppendLine(setting.Name + ": " + setting.Status);

                    return true;
                })
                .OrderBy(x => x.Name)
                .ToArray();

            stringBuilder.AppendLine("=================");

            foreach (ISetting setting in enabledSettings)
            {
                stringBuilder.AppendLine(setting.Name + ": " + setting.Status);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get all the <see cref="ISetting"/>s contained within <see cref="Commands"/>.
        /// </summary>
        /// <returns>all the <see cref="ISetting"/>s contained within <see cref="Commands"/>.</returns>
        public IEnumerable<ISetting> Settings => LazySettings.Value;

        /// <summary>
        /// Get all the <see cref="IUserCommand"/>s contained within <see cref="Commands"/>.
        /// </summary>
        /// <returns>all the <see cref="IUserCommand"/>s contained within <see cref="Commands"/>.</returns>
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
            ApplyMetaCommand(
                commandName,
                arguments,
                (command, property, arguments) => command.Set(property, arguments),
                commandName => $"Command \"{commandName}\" not found to set.",
                (commandName, property, arguments) => $"Command \"{commandName}\" property \"{property}\" set to \"{arguments}\".",
                (commandName, property) => $"Command \"{commandName}\" property \"{property}\" not set.");
        }

        private void Add(String commandName, IEnumerable<String> arguments)
        {
            ApplyMetaCommand(
                commandName,
                arguments,
                (command, property, arguments) => command.Add(property, arguments),
                commandName => $"Command \"{commandName}\" not found to add to.",
                (commandName, property, arguments) => $"Command \"{commandName}\" property \"{property}\" added \"{arguments}\".",
                (commandName, property) => $"Command \"{commandName}\" property \"{property}\" not added to.");
        }

        private void Remove(String commandName, IEnumerable<String> arguments)
        {
            ApplyMetaCommand(
                commandName,
                arguments,
                (command, property, arguments) => command.Remove(property, arguments),
                commandName => $"Command \"{commandName}\" not found to remove from.",
                (commandName, property, arguments) => $"Command \"{commandName}\" property \"{property}\" removed \"{arguments}\".",
                (commandName, property) => $"Command \"{commandName}\" property \"{property}\" not removed from.");
        }

        private void ApplyMetaCommand(
            String commandName,
            IEnumerable<String> arguments,
            Func<ICommand, String, IEnumerable<String>, Boolean> metaCommand,
            Func<String, String> notFoundMessage,
            Func<String, String, String, String> successMessage,
            Func<String, String, String> failureMessage)
        {
            ICommand commandToAddTo = GetCommand(commandName);
            String property = arguments.FirstOrDefault();

            arguments = arguments.Skip(1);

            if (commandToAddTo == null)
            {
                Console.WriteLine(notFoundMessage(commandName));
            }
            else if (metaCommand(commandToAddTo, property, arguments))
            {
                Console.WriteLine(successMessage(commandName, property, String.Join(" ", arguments)));
            }
            else
            {
                Console.WriteLine(failureMessage(commandName, property));
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

        private ICommand GetCommand(String commandName) => Commands.GetValueOrDefault(commandName.ToLower());
    }
}
