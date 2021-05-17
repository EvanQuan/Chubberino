using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public abstract class UserCommand : Setting, IUserCommand
    {
        public static IReadOnlyList<String> UserIdsToIgnore { get; } = new List<String>()
        {
            TwitchUserIDs.MoTroBo,
            TwitchUserIDs.ThePositiveBot,
            TwitchUserIDs.SimonSaysBot,
            TwitchUserIDs.VJBotardo,
            TwitchUserIDs.Nightbot,
            TwitchUserIDs.StreamElements,
            TwitchUserIDs.AmazefulBot,
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
            var allWords = message.Split(' ');
            if (allWords[0].Equals('!' + Name, StringComparison.OrdinalIgnoreCase))
            {
                words = allWords.Skip(1);
            }

            return words != null;
        }
    }
}
