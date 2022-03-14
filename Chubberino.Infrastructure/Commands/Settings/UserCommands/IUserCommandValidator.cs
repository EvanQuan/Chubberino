using System;
using TwitchLib.Client.Models;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public interface IUserCommandValidator
    {
        Boolean TryValidateCommand(ChatMessage message, out String userCommandName, out OnUserCommandReceivedArgs args);
    }
}
