using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings;
using Chubberino.Infrastructure.Commands.Settings.UserCommands;
using Chubberino.Infrastructure.Credentials;
using TwitchLib.Client.Events;

namespace Chubberino.Infrastructure.Commands
{
    public sealed class CommandRepository : ICommandRepository
    {
        public const String StatusLine = "=================";

        private TextWriter Writer { get; }

        public ITwitchClientManager ClientManager { get; }

        public IUserCommandValidator UserCommandValidator { get; }

        public SettingCollection<ISetting> Settings { get; }
        public Dictionary<String, ICommand> Commands { get; }
        public Dictionary<String, ICommand> RegularCommands { get; }
        public Dictionary<String, IUserCommand> UserCommands { get; }
        public ICommandConfigurationStrategy CommandConfigurationStrategy { get; }

        public CommandRepository(
            TextWriter writer,
            ITwitchClientManager clientManager,
            IUserCommandValidator userCommandValidator,
            ICommandConfigurationStrategy commandConfigurationStrategy)
        {
            Writer = writer;
            ClientManager = clientManager;
            UserCommandValidator = userCommandValidator;
            CommandConfigurationStrategy = commandConfigurationStrategy;
            Settings = new();
            Commands = new();
            RegularCommands = new();
            UserCommands = new();

            ClientManager.OnTwitchClientRefreshed += ClientManager_OnTwitchClientRefreshed;

            if (ClientManager.Client is not null)
            {
                ClientManager.Client.OnMessageReceived += Client_OnMessageReceived;
            }
        }

        private void ClientManager_OnTwitchClientRefreshed(Object sender, OnTwitchClientRefreshedArgs e)
        {
            if (e.OldClient.HasValue)
            {
                e.OldClient.Value.OnMessageReceived -= Client_OnMessageReceived;
            }
            e.NewClient.OnMessageReceived += Client_OnMessageReceived;
        }

        private void Client_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (UserCommandValidator.TryValidateCommand(e.ChatMessage, out var userCommandName, out var args))
            {
                if (UserCommands.TryGetValue(userCommandName, out var userCommand)
                    && userCommand.IsEnabled)
                {
                    userCommand.Invoke(this, args);
                }
            }
        }

        public ICommandRepository AddCommand(ICommand command)
        {
            Commands.Add(command.Name, command);

            if (command is ISetting setting)
            {
                Settings.AddDisabled(setting);

                if (command is IUserCommand userCommand)
                {
                    UserCommands.TryAdd(userCommand.Name, userCommand);
                }
            }
            else
            {
                RegularCommands.Add(command.Name, command);
            }

            return this;
        }

        /// <summary>
        /// Sets <see cref="ISetting.IsEnabled"/> to false for all settings.
        /// </summary>
        public void DisableAllSettings()
        {
            foreach (var setting in Settings.Enabled.Select(x => x.Name).ToArray())
            {
                Settings.Disable(setting);
            }
            DisableAllUserCommands();
        }

        public void DisableAllUserCommands()
        {
            foreach (var userCommand in UserCommands.Values.Select(x => x.Name))
            {
                Settings.Disable(userCommand);
            }
        }

        public String GetStatus()
        {
            StringBuilder stringBuilder = new();

            // Regular
            foreach (var regularCommand in RegularCommands.OrderBy(x => x.Key).Select(x => x.Value))
            {
                stringBuilder.AppendLine(regularCommand.Name);
            }

            stringBuilder.AppendLine(StatusLine);

            // Disabled settings are first.
            foreach (var setting in Settings.Disabled.OrderBy(x => x.Name))
            {
                stringBuilder.AppendLine(setting.Name + ": " + setting.Status);
            }

            stringBuilder.AppendLine(StatusLine);

            foreach (var setting in Settings.Enabled.OrderBy(x => x.Name))
            {
                stringBuilder.AppendLine(setting.Name + ": " + setting.Status);
            }

            return stringBuilder.ToString();
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

                    if (commandToExecute is null)
                    {
                        Writer.WriteLine($"Command \"{commandName}\" not found.");
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
            Writer.WriteLine(GetGetMessage(commandName, arguments));
        }

        private String GetGetMessage(String commandName, IEnumerable<String> arguments)
        {
            ICommand commandToSet = GetCommand(commandName);

            if (commandToSet is null)
            {
                return $"Command \"{commandName}\" not found to get.";
            }

            String value = commandToSet.Get(arguments);
            if (String.IsNullOrWhiteSpace(value))
            {
                return $"Command \"{commandName}\" value \"{String.Join(" ", arguments)}\" not found.";
            }

            return $"Command \"{commandName}\" value \"{String.Join(" ", arguments)}\" is \"{value}\".";
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

            String resultMessage;
            if (commandToAddTo is null)
            {
                resultMessage = notFoundMessage(commandName);
            }
            else if (metaCommand(commandToAddTo, property, arguments))
            {
                resultMessage = successMessage(commandName, property, String.Join(" ", arguments));
            }
            else
            {
                resultMessage = failureMessage(commandName, property);
            }

            Writer.WriteLine(resultMessage);
        }

        private void Help(String commandName)
        {

            ICommand commandToSet = GetCommand(commandName);

            String message = commandToSet?.GetHelp();

            if (message is not null)
            {
                Writer.WriteLine(message);
            }
            else
            {
                Writer.WriteLine($"Command \"{commandName}\" not found.");
            }
        }

        private ICommand GetCommand(String commandName) => Commands.GetValueOrDefault(commandName.ToLower());

        public ICommandRepository Disable(String settingName)
        {
            Settings.Disable(settingName);
            return this;
        }

        public ICommandRepository Enable(String settingName)
        {
            Settings.Enable(settingName);
            return this;
        }

        public void Configure(LoginCredentials loginCredentials)
        {
            CommandConfigurationStrategy.Configure(this, loginCredentials);
        }
    }
}
