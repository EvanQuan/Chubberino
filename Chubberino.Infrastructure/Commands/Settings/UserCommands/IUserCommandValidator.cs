using System;
using Chubberino.Common.ValueObjects;
using TwitchLib.Client.Models;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public interface IUserCommandValidator
    {
        Boolean TryValidateCommand(ChatMessage message, out Name userCommandName, out OnUserCommandReceivedArgs args);
    }
}
