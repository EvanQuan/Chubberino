using TwitchLib.Client.Models;

namespace Chubberino.Client.Commands.Settings.UserCommands;

public sealed class OnUserCommandReceivedArgs : EventArgs
{
    public OnUserCommandReceivedArgs(ChatMessage chatMessage, String[] words)
    {
        ChatMessage = chatMessage;
        Words = words;
    }

    public ChatMessage ChatMessage { get; }

    /// <summary>
    /// <see cref="ChatMessage.Message"/> split up into words without the
    /// initial command word.
    /// </summary>
    public String[] Words { get; }
}
