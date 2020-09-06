using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings;
using Chubberino.Client.Commands.Settings.Replies;
using Chubberino.Client.Commands.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chubberino.Client.Commands
{
    public sealed class CommandRepository : ICommandRepository
    {
        public IReadOnlyList<ICommand> Commands { get; }

        private CommandRepository()
        {
        }

        public CommandRepository(IExtendedClient client)
        {
            var stopSettingStrategy = new StopSettingStrategy();

            var commands = new List<ICommand>()
            {
                new AutoChat(client, stopSettingStrategy),
                new AutoPogO(client),
                new Color(client),
                new Copy(client),
                new Count(client, new Repeater()),
                new Greet(client),
                new Jimbox(client),
                new Join(client),
                new Log(client),
                new MockStreamElements(client),
                new Mode(client),
                new Repeat(client, new Repeater()),
                new Reply(client, new EqualsComparator(), new ContainsComparator()),
                new Say(client),
                new TimeoutAlert(client),
                new TrackJimbox(client),
                new TrackPyramids(client),
                new YepKyle(client),
                new DisableAll(client, this),
            };

            Commands = commands;
        }

        /// <summary>
        /// Sets <see cref="ISetting.IsEnabled"/> to false for all settings.
        /// </summary>
        public void DisableAllSettings()
        {
            IEnumerable<ISetting> settings = GetSettings();

            foreach (ISetting setting in settings)
            {
                setting.IsEnabled = false;
            }
        }

        public void RefreshAll(IExtendedClient twitchClient)
        {
            foreach (ICommand command in Commands)
            {
                command.Refresh(twitchClient);
            }
        }

        public String GetStatus()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (ICommand command in Commands)
            {
                if (command is ISetting setting)
                {
                    stringBuilder.Append(setting.Name + ": " + setting.Status + Environment.NewLine);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get all the <see cref="ISetting"/>s contained within <see cref="Commands"/>.
        /// </summary>
        /// <returns>all the <see cref="ISetting"/>s contained within <see cref="Commands"/>.</returns>
        public IEnumerable<ISetting> GetSettings()
        {
            var settingList = new List<ISetting>();

            foreach (ICommand command in Commands)
            {
                if (command is ISetting setting)
                {
                    settingList.Add(setting);
                }
            }

            return settingList;
        }

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

        private ICommand GetCommand(String commandName) => Commands
                .Where(command => command.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
    }
}
