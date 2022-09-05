using System;
using System.Linq;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Common.ValueObjects;
using Chubberino.Infrastructure.Client;
using TwitchLib.Client.Models;

namespace Chubberino.Infrastructure.Commands.Settings.UserCommands;

public sealed class UserCommandValidator : IUserCommandValidator
{
    /// <summary>
    /// TODO: Have channel configurable prefix
    /// </summary>
    public const Char CommandPrefix = '!';

    public Boolean TryValidateCommand(ChatMessage message, out Name userCommandName, out OnUserCommandReceivedArgs args)
    {
        userCommandName = default;
        args = default;

        if (ShouldIgnoreUser(message)) { return false; }

        var allWords = message.Message.Split(' ');
        var firstWord = allWords[0];

        if (!IsCommand(firstWord)) { return false; }

        userCommandName = Name.From(firstWord[1..].ToLower());

        var words = allWords.Skip(1).ToArray();
        args = new OnUserCommandReceivedArgs(message, words);

        return true;
    }

    private static Boolean IsCommand(String firstWord)
    {
        return firstWord.StartsWith(CommandPrefix) && firstWord.Length >= 2;
    }

    private static Boolean ShouldIgnoreUser(ChatMessage message)
    {
        return TwitchUserIDs.ChannelBots.Contains(message.UserId);
    }
}
