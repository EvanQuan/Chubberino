using Chubberino.Common.Services;
using Chubberino.Common.ValueObjects;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;
using Chubberino.Infrastructure.Credentials;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.Infrastructure.Client
{
    public sealed class Bot : IBot
    {
        public BotState State { get; set; }
        public TextWriter Writer { get; }
        private ICommandRepository Commands { get; set; }

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

        public LoginCredentials LoginCredentials { get; set; }

        public Bot(
            TextWriter writer,
            ICommandRepository commands,
            IModeratorClientOptions moderatorOptions,
            IRegularClientOptions regularOptions,
            ITwitchClientManager twitchClientManager)
        {
            Writer = writer;
            Commands = commands;
            ModeratorClientOptions = moderatorOptions;
            RegularClientOptions = regularOptions;
            TwitchClientManager = twitchClientManager;
            IsModerator = true;
        }

        public LoginCredentials Initialize(LoginCredentials credentials)
        {
            credentials = TwitchClientManager.TryInitializeTwitchClient(this, credentials: credentials);

            if (credentials == null)
            {
                return null;
            }

            if (!TwitchClientManager.TryJoinInitialChannels())
            {
                return null;
            }

            return credentials;
        }

        public LoginCredentials Start(
            IClientOptions clientOptions = null,
            LoginCredentials credentials = null)
        {
            IReadOnlyList<JoinedChannel> previouslyJoinedChannels = TwitchClientManager.Client?.JoinedChannels;

            LoginCredentials = TwitchClientManager.TryInitializeTwitchClient(this, clientOptions, credentials);
            if (LoginCredentials is not null && TwitchClientManager.TryJoinInitialChannels(previouslyJoinedChannels))
            {
                Commands.Configure(LoginCredentials);

                return LoginCredentials;
            }

            return null;
        }

        public String GetPrompt()
        {
            return Environment.NewLine + Environment.NewLine + Commands.GetStatus() + Environment.NewLine
                + $"[{LoginCredentials.ConnectionCredentials.TwitchUsername} as {(IsModerator ? "Mod" : "Normal")} in {TwitchClientManager.PrimaryChannelName}]> ";
        }



        public void Refresh(IClientOptions clientOptions = null)
        {
            // TODO: Is this needed if the whole program will restart after?
            LoginCredentials = Start(clientOptions, LoginCredentials);

            Boolean successful = LoginCredentials != null;
            if (successful)
            {
                Writer.WriteLine("Refresh successful");
            }
            else
            {
                Writer.WriteLine("Failed to refresh");
                State = BotState.ShouldStop;
            }
        }

        public void ReadCommand(String command)
        {
            try
            {
                String[] arguments = command.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (arguments.Length == 0) { return; }

                var commandName = Name.From(arguments[0].ToLower());

                switch (commandName.Value)
                {
                    case "quit":
                        State = BotState.ShouldStop;
                        break;

                    default:
                        Commands.Execute(commandName, arguments.Skip(1));
                        break;
                }
            }
            catch (Exception e)
            {
                Writer.WriteLine("Exception thrown: " + e.Message);
                Writer.WriteLine("Restarting");
                Refresh(ModeratorClientOptions);
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
