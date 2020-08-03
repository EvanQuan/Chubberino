using Chubberino.Client.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class AutoChat : Setting
    {
        private ConcurrentQueue<String> PreviousMessages { get; }

        private Int32 SpamMessageCount { get; set; } = 10;

        private Int32 GeneralMessageCount { get; set; } = 25;

        public override String Status => base.Status
            + $"\n\tSpam count: {SpamMessageCount}"
            + $"\n\tGeneral count: {GeneralMessageCount}";

        public AutoChat(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            PreviousMessages = new ConcurrentQueue<String>();
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!IsEnabled) { return; }

            if (!ShouldCountUser(e.ChatMessage)) { return; }

            PreviousMessages.Enqueue(e.ChatMessage.Message);

            if (PreviousMessages.Count >= GeneralMessageCount)
            {
                // Get the most common message from the last GeneralMessageCount
                // number of messages, as long as there is more than one
                // duplicate of it.
                String message = GetMessageToSend(3);

                if (message != null)
                {
                    Spooler.SpoolMessage(message);
                }

                // Empty the last messages, whether one was sent or not to
                // avoid copying old messages.
                PreviousMessages.Clear();
            }
            else if (PreviousMessages.Count >= SpamMessageCount)
            {
                // Get the most common message from the last SpamMessageCount
                // number of messages, as long as there is more than one
                // duplicate of it.
                String message = GetMessageToSend(4);

                if (message != null)
                {
                    Spooler.SpoolMessage(message);
                    // Empty the last messages, if if one was sent.
                    PreviousMessages.Clear();
                }

            }
        }

        private String GetMessageToSend(Int32 duplicateCount)
        {
            return PreviousMessages
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
                .Where(x => x.Count() >= duplicateCount)
                .Select(x => x.Key)
                .FirstOrDefault();
        }

        private Boolean ShouldCountUser(ChatMessage chatMessage)
        {
            return !chatMessage.Username.Equals(chatMessage.BotUsername)
                && !chatMessage.IsModerator
                && !chatMessage.IsVip
                && !chatMessage.Message.StartsWith('!') // Ignore channel bot commands
                && !chatMessage.Message.Contains(TwitchInfo.BotUsername, StringComparison.OrdinalIgnoreCase);
        }
    }
}
