using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public abstract class UserCommand : Setting, IUserCommand
    {
        private static IReadOnlyList<String> UserIdsToIgnore { get; } = new List<String>()
        {
            // Nightbot
            "19264788",
            // SimonSaysBot,
            "469718952",
            // VJBotardo
            "500670723",
        };

        protected UserCommand(ITwitchClientManager client, IConsole console) : base(client, console)
        {
        }

        /// <summary>
        /// Tries to validate whether the message received successfully triggers this command.
        /// </summary>
        /// <param name="args">On message received arguments.</param>
        /// <param name="words"></param>
        /// <returns></returns>
        protected Boolean TryValidateCommand(OnMessageReceivedArgs args, out IEnumerable<String> words)
        {
            words = null;

            if (UserIdsToIgnore.Contains(args.ChatMessage.UserId)) { return false; }
            String message = args.ChatMessage.Message;

            if (message.StartsWith('!' + Name, StringComparison.OrdinalIgnoreCase))
            {
                words = message.Split(' ').Skip(1);
            }

            return words != null;
        }
    }
}
