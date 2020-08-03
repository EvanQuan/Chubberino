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

        private IEnumerable<Int32> SpamMessageCount { get; set; } = new Int32[] { 30, 25, 18 };

        private Int32 GeneralMessageCount { get; set; } = 35;

        private Int32 MinimumDuplicateCount { get; set; } = 3;

        public AutoChat(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnHostingStarted += TwitchClient_OnHostingStarted;
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            PreviousMessages = new ConcurrentQueue<String>();
        }

        private void TwitchClient_OnHostingStarted(Object sender, OnHostingStartedArgs e)
        {
            // Stop when stream ends and hosting another channel.
            Console.WriteLine($"!!! {e.HostingStarted.HostingChannel} started hosting {e.HostingStarted.TargetChannel}. Stopped autochat.");
            IsEnabled = false;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!IsEnabled) { return; }

            if (ShouldIgnore(e.ChatMessage)) { return; }

            PreviousMessages.Enqueue(e.ChatMessage.Message);

            if (PreviousMessages.Count >= GeneralMessageCount)
            {
                // Get the most common message from the last GeneralMessageCount
                // number of messages, as long as there is more than one
                // duplicate of it.
                String message = GetMessageToSend();

                if (message != null)
                {
                    Spooler.SpoolMessage(message);
                }

                // Empty the last messages, whether one was sent or not to
                // avoid copying old messages.
                PreviousMessages.Clear();
            }
            else if (SpamMessageCount.Any(count => PreviousMessages.Count >= count))
            {
                // Get the most common message from the last SpamMessageCount
                // number of messages, as long as there is more than one
                // duplicate of it.
                String message = GetMessageToSend();

                if (message != null)
                {
                    Spooler.SpoolMessage(message);
                    // Empty the last messages, if if one was sent.
                    PreviousMessages.Clear();
                }

            }
        }

        private String GetMessageToSend()
        {
            return PreviousMessages
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
                .Where(x => x.Count() >= MinimumDuplicateCount)
                .Select(x => x.Key)
                .FirstOrDefault();
        }

        /// <summary>
        /// Should ignore message. These messages should not be copied.
        /// </summary>
        /// <param name="chatMessage">Chat message.</param>
        /// <returns>true if should ignore <paramref name="chatMessage"/>; false otherwise.</returns>
        private Boolean ShouldIgnore(ChatMessage chatMessage)
        {
            return chatMessage.Username.Equals(chatMessage.BotUsername) // Don't copy messages from self
                || chatMessage.IsModerator // Ignore channel bot responds, or mod spam (such as for links)
                || chatMessage.IsVip // VIP spam may time you out
                || chatMessage.Message.StartsWith('!') // Ignore channel bot commands
                || chatMessage.Message.Contains(TwitchInfo.BotUsername, StringComparison.OrdinalIgnoreCase); // If being @'d, don't want to copy @ ing self.
        }
    }
}
