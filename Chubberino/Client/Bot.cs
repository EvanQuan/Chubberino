using Chubberino.Client.Commands;
using Chubberino.Client.Credentials;
using Chubberino.Client.Threading;
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

        public LoginCredentials LoginCredentials { get; set; }

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

        public LoginCredentials Start(
            IClientOptions clientOptions = null,
            LoginCredentials credentials = null)
        {
            IReadOnlyList<JoinedChannel> previouslyJoinedChannels = TwitchClientManager.Client?.JoinedChannels;

            LoginCredentials = TwitchClientManager.TryInitialize(this, clientOptions, credentials);
            if (LoginCredentials != null && TwitchClientManager.TryJoinInitialChannels(previouslyJoinedChannels))
            {
                return LoginCredentials;
            }

            return null;
        }

        public String GetPrompt()
        {
            return Environment.NewLine + Environment.NewLine + Commands.GetStatus() + Environment.NewLine
                + $"[{Name} | {(IsModerator ? "Mod" : "Normal")} {TwitchClientManager.PrimaryChannelName}]> ";
        }



        public void Refresh(IClientOptions clientOptions = null)
        {
            LoginCredentials = Start(clientOptions, LoginCredentials);

            Boolean successful = LoginCredentials != null;
            if (successful)
            {
                Commands.RefreshAll();
                Console.WriteLine("Refresh successful");
            }
            else
            {
                Console.WriteLine("Failed to refresh");
                State = BotState.ShouldRestart;
            }
        }

        public void ReadCommand(String command)
        {
            try
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
            catch
            {
                State = BotState.ShouldRestart;
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
