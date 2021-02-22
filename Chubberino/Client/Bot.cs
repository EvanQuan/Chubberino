using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Client
{
    public sealed class Bot : IBot
    {
        public BotState State { get; set; }

        private ICommandRepository Commands { get; set; }

        private IConsole Console { get; set; }

        /// <summary>
        /// Twitch user name that the bot is logged into.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 100 messages in 30 seconds ~1 message per 0.3 seconds.
        /// </summary>
        public IModeratorClientOptions ModeratorClientOptions { get; }

        /// <summary>
        /// 20 messages in 30 seconds ~1 message per 1.5 second
        /// </summary>
        public IRegularClientOptions RegularClientOptions { get; }

        /// <summary>
        /// Is the bot a broadcaster/moderator/VIP?
        /// </summary>
        public Boolean IsModerator { get; set; }

        private ITwitchClientManager TwitchClientManager { get; }

        public ISpinWait SpinWait { get; }

        public Bot(
            IConsole console,
            ICommandRepository commands,
            IModeratorClientOptions moderatorOptions,
            IRegularClientOptions regularOptions,
            ITwitchClientManager twitchClientManager,
            ISpinWait spinWait)
        {
            Console = console;
            Commands = commands;
            ModeratorClientOptions = moderatorOptions;
            RegularClientOptions = regularOptions;
            TwitchClientManager = twitchClientManager;
            SpinWait = spinWait;
            IsModerator = true;
        }

        public Boolean Start(
            IClientOptions clientOptions = null,
            Boolean askForCredentials = true)
        {
            IReadOnlyList<JoinedChannel> previouslyJoinedChannels = TwitchClientManager.Client?.JoinedChannels;

            return TwitchClientManager.TryInitialize(this, clientOptions, askForCredentials)
                 && TwitchClientManager.TryJoinInitialChannels(previouslyJoinedChannels);
        }

        public String GetPrompt()
        {
            return Environment.NewLine + Environment.NewLine + Commands.GetStatus() + Environment.NewLine
                + $"[{Name} | {(IsModerator ? "Mod" : "Normal")} {TwitchClientManager.PrimaryChannelName}]> ";
        }



        public void Refresh(
            IClientOptions clientOptions = null,
            Boolean askForCredentials = true)
        {
            Boolean successful = Start(clientOptions, askForCredentials);

            if (successful)
            {
                Commands.RefreshAll();
                Console.WriteLine("Refresh " + (successful ? "successful" : "failed"));
            }
            else
            {
                Console.WriteLine("Failed to refresh");
                State = BotState.ShouldStop;
            }
        }

        public void ReadCommand(String command)
        {
            String[] arguments = command.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (arguments.Length == 0) { return; }

            String commandName = arguments[0].ToLower();

            switch (commandName)
            {
                case "quit":
                    State = BotState.ShouldStop;
                    break;

                default:
                    Commands.Execute(commandName, arguments.Skip(1));
                    break;
            }
        }

        public void Dispose()
        {
            if (TwitchClientManager.Client?.IsConnected ?? false)
            {
                TwitchClientManager.Client.Disconnect();
            }
        }
    }
}
