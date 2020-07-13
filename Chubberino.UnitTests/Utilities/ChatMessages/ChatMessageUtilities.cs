using System;
using TwitchLib.Client.Models;

namespace Chubberino.UnitTests.Utilities.ChatMessages
{
    public static class ChatMessageUtilities
    {
        public static ChatMessage GetChatMessage(String username, String message)
            => new ChatMessage(
                    botUsername: default,
                    userId: default,
                    userName: username,
                    displayName: default,
                    colorHex: default,
                    color: default,
                    emoteSet: default,
                    message: message,
                    userType: default,
                    channel: default,
                    id: default,
                    isSubscriber: default,
                    subscribedMonthCount: default,
                    roomId: default,
                    isTurbo: default,
                    isModerator: default,
                    isMe: default,
                    isBroadcaster: default,
                    noisy: default,
                    rawIrcMessage: default,
                    emoteReplacedMessage: default,
                    badges: default,
                    cheerBadge: default,
                    bits: default,
                    bitsInDollars: default);
    }
}
