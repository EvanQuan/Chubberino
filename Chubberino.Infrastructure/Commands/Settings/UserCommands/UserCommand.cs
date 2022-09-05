using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using TwitchLib.Client.Events;

namespace Chubberino.Infrastructure.Commands.Settings.UserCommands;

public abstract class UserCommand : Setting, IUserCommand
{
    protected UserCommand(ITwitchClientManager client, TextWriter writer) : base(client, writer)
    {
    }

    /// <summary>
    /// Tries to validate whether the message received successfully triggers this command.
    /// </summary>
    /// <param name="args">On message received arguments.</param>
    /// <param name="words"></param>
    /// <returns></returns>
    [Obsolete("Move responsibility to CommandRepository")]
    protected Boolean TryValidateCommand(OnMessageReceivedArgs args, out IEnumerable<String> words)
    {
        words = null;

        if (TwitchUserIDs.ChannelBots.Contains(args.ChatMessage.UserId)) { return false; }

        String message = args.ChatMessage.Message;
        var allWords = message.Split(' ');
        if (allWords[0].Equals('!' + Name.Value, StringComparison.OrdinalIgnoreCase))
        {
            words = allWords.Skip(1);
        }

        return words != null;
    }

    public abstract void Invoke(Object sender, OnUserCommandReceivedArgs e);
}
