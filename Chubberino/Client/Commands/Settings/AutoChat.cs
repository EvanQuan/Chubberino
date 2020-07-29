using Chubberino.Client.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class AutoChat : Setting
    {
        private ConcurrentQueue<String> PreviousMessages { get; }

        private Int32 MessageCount { get; set; } = 25;

        public override String Status => base.Status
            + $"\n\tCount: {MessageCount}";

        public AutoChat(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            PreviousMessages = new ConcurrentQueue<string>();
        }

        private void TwitchClient_OnMessageReceived(Object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            if (!IsEnabled) { return; }

            if (!ShouldCountUser(e.ChatMessage)) { return; }

            PreviousMessages.Enqueue(e.ChatMessage.Message);

            if (PreviousMessages.Count >= MessageCount)
            {
                // Get the most common message from the last MessageCount
                // number of messages, as long as there is more than one
                // duplicate of it.
                String message = PreviousMessages
                    .GroupBy(x => x)
                    .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Key)
                    .FirstOrDefault();

                if (message != null)
                {
                    Spooler.SpoolMessage(message);
                }

                // Empty the last messages, whether one was sent or not to
                // avoid copying old messages.
                PreviousMessages.Clear();
            }
        }

        public override Boolean Set(String property, IEnumerable<String> arguments)
        {
            switch (property)
            {
                case "c":
                case "count":
                    if (Int32.TryParse(arguments.FirstOrDefault(), out Int32 result))
                    {
                        MessageCount = result;
                        return true;
                    }
                    return false;
            }

            return false;
        }

        private Boolean ShouldCountUser(ChatMessage chatMessage)
        {
            return !chatMessage.IsMe
                && !chatMessage.IsModerator
                && !chatMessage.IsVip
                && !chatMessage.Message.StartsWith('!'); // Ignore channel bot commands
        }
    }
}
