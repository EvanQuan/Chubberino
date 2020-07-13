using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    internal sealed class Reply : Setting
    {
        private String ReplyMessage { get; set; }
        private String CompareType { get; set; }
        private Func<String, String, Boolean> Comparator { get; set; }

        public override String Status => String.IsNullOrWhiteSpace(ReplyMessage)
            ? "disabled"
            : $"{CompareType} \"{ReplyMessage}\"";

        public Reply(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!IsEnabled) { return; }

            if (Comparator(e.ChatMessage.Message, ReplyMessage))
            {
                TwitchClient.SendMessage(e.ChatMessage.Channel, $"@{e.ChatMessage.Username} {ReplyMessage}");
            }
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            IsEnabled = arguments.Count() > 0;

            if (IsEnabled)
            {
                // Check for comparator
                String comparatorString = arguments.FirstOrDefault();

                switch (comparatorString.ToLower())
                {
                    case "contains":
                        Comparator = (actualMessage, replyMessage) => actualMessage.Contains(replyMessage);
                        CompareType = "contain";
                        ReplyMessage = String.Join(" ", arguments.Skip(1));
                        break;
                    default:
                        Comparator = (actualMessage, replyMessage) => actualMessage == replyMessage;
                        CompareType = "equal";
                        ReplyMessage = String.Join(" ", arguments.Skip(1));
                        break;
                }

                Console.WriteLine($"Replying to messages that {CompareType} \"{ReplyMessage}\"");
            }
            else
            {
                ReplyMessage = null;
                Console.WriteLine("Reply disabled");
            }
        }

        public override String Help()
        {
            return @"
Reply to any message that matches to a specified message, by copying the
message back.

usage: reply [contains] <message>

    contains - replies to messages that contain <message> as a substring, as
               opposed to being equal to <message>

    message - the message to check against whether to reply to.
";
        }
    }
}
