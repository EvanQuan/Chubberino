using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public abstract class UserCommand : Setting, IUserCommand
    {
        protected UserCommand(IExtendedClient client, TextWriter console) : base(client, console)
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
            String message = args.ChatMessage.Message;   

            words =  message.StartsWith('!' + Name, StringComparison.OrdinalIgnoreCase)
                ? message.Split(' ').Skip(1)
                : null;

            return words != null;
        }
    }
}
