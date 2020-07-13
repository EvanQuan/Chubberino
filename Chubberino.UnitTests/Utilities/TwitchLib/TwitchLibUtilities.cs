using System;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace Chubberino.UnitTests.Utilities.TwitchLib
{
    public static class TwitchLibUtilities
    {
        public static OnMessageReceivedArgs GetOnMessageReceivedArgs(String username, String message)
            => new OnMessageReceivedArgs()
            {
                ChatMessage = GetChatMessage(username, message)
            };

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
