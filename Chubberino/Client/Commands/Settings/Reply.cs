using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    internal sealed class Reply : Setting
    {
        /// <summary>
        /// The specified users to reply to. If empty, reply to any user.
        /// </summary>
        private HashSet<String> UsersToReplyTo { get; set; }

        /// <summary>
        /// The message to trigger a reply.
        /// </summary>
        private String TriggerMessage { get; set; }

        /// <summary>
        /// The message to reply with.
        /// If null, reply with "@<see cref="UserToReplyTo"/> <see cref="TriggerMessage"/>".
        /// </summary>
        private String ReplyMessage { get; set; }

        /// <summary>
        /// String representation of the comparator.
        /// </summary>
        private String CompareType { get; set; }

        /// <summary>
        /// The function that compares the received message with the trigger
        /// message. If the comparator returns true, the received message
        /// matches.
        /// </summary>
        private Func<String, String, Boolean> Comparator { get; set; }

        private Func<String, String, Boolean> EqualsComparator { get; }
            = (actualMessage, triggerMessage) => actualMessage == triggerMessage;

        /// <summary>
        /// The 
        /// </summary>
        private Func<String, String, Boolean> ContainsComparator { get; }
            = (actualMessage, triggerMessage) => triggerMessage.Contains(actualMessage);

        public override String Status => new StringBuilder()
            .AppendLine((IsEnabled ? "enabled" : "disabled"))
            .AppendLine($"\tmessage: {TriggerMessage}\n")
            .AppendLine($"\tcomparator: {CompareType}\n")
            .AppendLine($"\tusers: {(UsersToReplyTo.Count == 0 ? "ANY" : "")}")
            .ToString();

        public Reply(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            UsersToReplyTo = new HashSet<String>();
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!IsEnabled) { return; }

            if (Comparator(e.ChatMessage.Message, TriggerMessage))
            {
                Spooler.SpoolMessage($"@{e.ChatMessage.Username} {TriggerMessage}");
            }
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            IsEnabled = arguments.Count() > 0;

            if (IsEnabled)
            {
                TriggerMessage = String.Join(" ", arguments);

                Console.WriteLine($"Replying to any message that {CompareType} \"{TriggerMessage}\"");
            }
            else
            {
                TriggerMessage = null;
                Console.WriteLine("Reply disabled");
            }
        }

        public override Boolean Set(String property, IEnumerable<String> arguments)
        {
            switch (property.ToLower())
            {
                case "u":
                case "user":
                    switch (arguments.FirstOrDefault())
                    {

                        case "a":
                        case "add":
                            String userToAdd = arguments.Skip(1).FirstOrDefault();
                            if (userToAdd != null)
                            {
                                UsersToReplyTo.Add(userToAdd);
                            }
                            break;
                        case "r":
                        case "remove":
                            String userToRemove = arguments.Skip(1).FirstOrDefault();
                            if (userToRemove != null)
                            {
                                UsersToReplyTo.Remove(userToRemove);
                            }
                            break;
                        case "c":
                        case "clear":
                            UsersToReplyTo.Clear();
                            break;
                    }
                    break;
                case "c":
                case "compare":
                case "comparator":
                    switch (arguments.FirstOrDefault())
                    {
                        case "c":
                        case "contain":
                        case "contains":
                            Comparator = (actualMessage, replyMessage) => actualMessage.Contains(replyMessage);
                            CompareType = "contains";
                            break;
                        case "e":
                        case "equal":
                        case "equals":
                        default:
                            Comparator = (actualMessage, replyMessage) => actualMessage == replyMessage;
                            CompareType = "equals";
                            break;
                    }
                    break;

                default:
                    return false;
            }

            return true;

        }

        public override String GetHelp()
        {
            return @"
Reply to any message that matches to a specified message, by copying the
message back.

usage: reply

    toggles enabled/disabled


set:
    message - the trigger message to check against whether to reply to.

    user [add|remove|clear]     - The users to reply to. If blank, will reply
                                  to any user.
        add (default)           - Add the specified user.
        remove                  - Remove the specified user.
        clear                   - Remove all listed users.

    comparator [equals|contains] - Specifies how to use compare the trigger
                                   message with any incoming messages

        equals (default) - Replies to messages that are exactly
                           equal (case sensitive) to the trigger
                           message.
        contains         - Replies to messages that contain
                           <message> as a substring, as opposed to
                           being equal to <message>.

    with - the message to reply with when a success match with the trigger
           message has been found. By default, '@ <message>', where @ is
           '@userNameToReplyTo'.
";
        }
    }
}
