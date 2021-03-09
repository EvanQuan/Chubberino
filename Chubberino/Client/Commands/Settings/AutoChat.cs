using Chubberino.Client.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class AutoChat : Setting
    {
        private ConcurrentQueue<String> PreviousMessages { get; }

        private IEnumerable<UInt32> SpamMessageSampleCounts { get; set; } = new UInt32[]
        {
            25,
            30,
        };

        private UInt32 MessageCooldownCount { get; set; } = 20;

        private UInt32 MessageCooldownTracker { get; set; }

        private Boolean OnCooldown { get; set; } = false;

        /// <summary>
        /// The number of messages to sample before determining what message to
        /// output.
        /// </summary>
        private UInt32 MessageSampleCount { get; set; } = 40;

        private UInt32 MinimumDuplicateCount { get; set; } = 3;

        public override String Status => base.Status
            + $"\n\tSample count: {MessageSampleCount}"
            + $"\n\tDuplicate count: {MinimumDuplicateCount}"
            + $"\n\tCooldown count: {MessageCooldownCount}";

        public IBot Bot { get; }

        public AutoChat(ITwitchClientManager client, IConsole console, IBot bot)
            : base(client, console)
        {
            PreviousMessages = new ConcurrentQueue<String>();
            Enable = twitchClient =>
            {
                twitchClient.OnHostingStarted += TwitchClient_OnHostingStarted;
                twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            };
            Disable = twitchClient =>
            {
                twitchClient.OnHostingStarted -= TwitchClient_OnHostingStarted;
                twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
            };
            Bot = bot;
        }

        public override Boolean Set(String property, IEnumerable<String> arguments)
        {
            switch (property.ToLower())
            {
                case "d":
                case "duplicate":
                    if (UInt32.TryParse(arguments.FirstOrDefault(), out UInt32 duplicateCount))
                    {
                        MinimumDuplicateCount = duplicateCount;
                        return true;
                    }
                    break;
                case "s":
                case "sample":
                    if (UInt32.TryParse(arguments.FirstOrDefault(), out UInt32 sampleCount))
                    {
                        MessageSampleCount = sampleCount;
                        return true;
                    }
                    break;
                case "c":
                case "cooldown":
                    if (UInt32.TryParse(arguments.FirstOrDefault(), out UInt32 cooldownLimit))
                    {
                        MessageCooldownCount = cooldownLimit;
                        return true;
                    }
                    break;
            }
            return false;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            base.Execute(arguments);

            if (!IsEnabled)
            {
                PreviousMessages.Clear();
            }
        }

        private void TwitchClient_OnHostingStarted(Object sender, OnHostingStartedArgs e)
        {
            // Stop when stream ends and hosting another channel.
            Console.WriteLine($"!!! {e.HostingStarted.HostingChannel} started hosting {e.HostingStarted.TargetChannel}. Stopped autochat.");
            IsEnabled = false;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (ShouldIgnore(e.ChatMessage))
            {
                PreviousMessages.Enqueue(null);
            }
            else if (OnCooldown)
            {
                MessageCooldownTracker++;
                if (MessageCooldownTracker > MessageCooldownCount)
                {
                    OnCooldown = false;
                    MessageCooldownTracker = 0;
                }
            }
            else
            {
                PreviousMessages.Enqueue(e.ChatMessage.Message);
            }


            if (PreviousMessages.Count >= MessageSampleCount)
            {
                // Get the most common message from the last GeneralMessageCount
                // number of messages, as long as there is more than one
                // duplicate of it.
                if (TryGetMessageToSend(out String message))
                {
                    TwitchClientManager.SpoolMessage(message);
                    OnCooldown = true;
                }

                // Empty the last messages, whether one was sent or not to
                // avoid copying old messages.
                PreviousMessages.Clear();
            }
            else if (SpamMessageSampleCounts.Any(count => PreviousMessages.Count >= count))
            {
                // Get the most common message from the last SpamMessageCount
                // number of messages, as long as there is more than one
                // duplicate of it.
                if (TryGetMessageToSend(out String message))
                {
                    TwitchClientManager.SpoolMessage(message);
                    // Empty the last messages, if one was sent.
                    PreviousMessages.Clear();
                    OnCooldown = true;
                }
            }
        }

        private Boolean TryGetMessageToSend(out String message)
        {
            message = PreviousMessages
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
                .Where(x => x.Count() >= MinimumDuplicateCount)
                .Select(x => x.Key)
                .FirstOrDefault();

            return message != default;
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
                || chatMessage.Message.Contains(Bot.Name, StringComparison.OrdinalIgnoreCase); // If being @'d, don't want to copy @ ing self.
        }
    }
}
