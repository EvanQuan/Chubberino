using System.IO;
using Chubberino.Bots.Common.Commands.Settings.Replies;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Bots.Common.Commands.Settings;

public sealed class Reply : Setting
{
    /// <summary>
    /// The specified users to reply to. If empty, reply to any user.
    /// </summary>
    private System.Collections.Generic.HashSet<String> UsersToReplyTo { get; set; }

    /// <summary>
    /// The message to trigger a reply.
    /// </summary>
    private String TriggerMessage { get; set; } = String.Empty;

    /// <summary>
    /// The message to reply with.
    /// If null, reply with "@<see cref="UserToReplyTo"/> <see cref="TriggerMessage"/>".
    /// </summary>
    private String ReplyMessage { get; set; }

    /// <summary>
    /// The function that compares the received message with the trigger
    /// message. If the comparator returns true, the received message
    /// matches.
    /// </summary>
    private IStringComparator Comparator { get; set; }

    private IEqualsComparator EqualsComparator { get; }

    private IContainsComparator ContainsComparator { get; }

    public override String Status => new StringBuilder()
        .AppendLine(base.Status)
        .AppendLine($"\tto: {(String.IsNullOrWhiteSpace(TriggerMessage) ? "< Any message >" : TriggerMessage)}\n")
        .AppendLine($"\tcomparator: {Comparator.Name}\n")
        .AppendLine($"\twith: {(String.IsNullOrWhiteSpace(ReplyMessage) ? "< Mirroring user message >" : ReplyMessage)}\n")
        .AppendLine($"\tusers: {(UsersToReplyTo.Count == 0 ? "< Any user >" : "\n\t\t" + String.Join("\n\t\t", UsersToReplyTo))}")
        .ToString();

    public Reply(ITwitchClientManager client, IEqualsComparator equalsComparator, IContainsComparator containsComparator, TextWriter writer)
        : base(client, writer)
    {
        UsersToReplyTo = new();
        EqualsComparator = equalsComparator;
        ContainsComparator = containsComparator;
        Comparator = ContainsComparator;
    }

    public override void Register(ITwitchClient client)
        => client.OnMessageReceived += TwitchClient_OnMessageReceived;

    public override void Unregister(ITwitchClient client)
        => client.OnMessageReceived -= TwitchClient_OnMessageReceived;

    private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
    {
        if (UsersToReplyTo.Any() && !UsersToReplyTo.Contains(e.ChatMessage.Username)) { return; }

        if (!Comparator.Matches(e.ChatMessage.Message, TriggerMessage)) { return; }

        String replyMessage = String.IsNullOrWhiteSpace(ReplyMessage) ? TriggerMessage : ReplyMessage;

        String replyMessageWithUserName = replyMessage.Replace("@", $"@{e.ChatMessage.DisplayName}");

        TwitchClientManager.SpoolMessage(replyMessageWithUserName);
    }

    public override Boolean Set(String property, IEnumerable<String> arguments)
    {
        switch (property.ToLower())
        {
            case "u":
            case "user":
            case "users":
                switch (arguments.FirstOrDefault())
                {
                    case "c":
                    case "clear":
                        UsersToReplyTo.Clear();
                        break;
                }
                break;
            case "c":
            case "compare":
            case "comparator":
                Comparator = arguments.FirstOrDefault() switch
                {
                    "c" or "contain" or "contains" => ContainsComparator,
                    _ => EqualsComparator,
                };
                break;

            case "w":
            case "with":
                // If there are no arguments, this will return empty string,
                // indicating the default behaviour of mirror the trigger
                // message.
                ReplyMessage = String.Join(' ', arguments);
                return true;

            case "t":
            case "to":
                // If there are no arguments, this will return empty string,
                // indicating the default behaviour of replying to any
                // message.
                TriggerMessage = String.Join(' ', arguments);
                return true;

            default:
                return false;
        }

        return true;
    }

    public override Boolean Add(String property, IEnumerable<String> arguments)
    {
        switch (property)
        {
            case "u":
            case "user":
            case "users":
                Int32 beforeCount = UsersToReplyTo.Count;
                foreach (String username in arguments)
                {
                    UsersToReplyTo.Add(username);
                }
                return beforeCount != UsersToReplyTo.Count;
            default:
                return false;
        }
    }

    public override Boolean Remove(String property, IEnumerable<String> arguments)
    {
        switch (property)
        {
            case "u":
            case "user":
            case "users":
                Int32 beforeCount = UsersToReplyTo.Count;
                foreach (String username in arguments)
                {
                    UsersToReplyTo.Remove(username);
                }
                return beforeCount != UsersToReplyTo.Count;
            default:
                return false;
        }
    }

    public override String GetHelp()
        => @"
Reply to any message that matches to a specified message, by copying the
message back.

usage: reply

    toggles enabled/disabled


set:
    to - the trigger message to check against whether to reply to.

    user                        - The users to reply to. If blank, will reply
                                  to any user.

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

add:
    user        - The users to reply to.

remove:
    user        - The users to reply to.
";
}
