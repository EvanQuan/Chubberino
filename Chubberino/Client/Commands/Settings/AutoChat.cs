using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Strategies;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        private IStopSettingStrategy StopSettingStrategy { get; }

        /// <summary>
        /// The number of messages to sample before determining what message to
        /// output.
        /// </summary>
        private UInt32 MessageSampleCount { get; set; } = 40;

        private UInt32 MinimumDuplicateCount { get; set; } = 4;

        public override String Status => base.Status
            + $"\n\tSample count: {MessageSampleCount}"
            + $"\n\tDuplicate count: {MinimumDuplicateCount}";

        public AutoChat(IExtendedClient client, IStopSettingStrategy stopSettingStrategy)
            : base(client)
        {
            PreviousMessages = new ConcurrentQueue<String>();
            StopSettingStrategy = stopSettingStrategy;
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
            if (ShouldStop(e.ChatMessage)) { return; }

            if (ShouldIgnore(e.ChatMessage))
            {
                PreviousMessages.Enqueue(null);
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
                String message = GetMessageToSend();

                if (message != null)
                {
                    TwitchClient.SpoolMessage(message);
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
                String message = GetMessageToSend();

                if (message != null)
                {
                    TwitchClient.SpoolMessage(message);
                    // Empty the last messages, if if one was sent.
                    PreviousMessages.Clear();
                }
            }
        }

        private String FilterMessage(String message)
        {
            return message;
        }

        private Boolean ShouldStop(ChatMessage chatMessage)
        {
            if (StopSettingStrategy.ShouldStop(chatMessage))
            {
                IsEnabled = false;
                Console.WriteLine("! ! ! STOPPED AUTOCHAT ! ! !");
                Console.WriteLine($"Moderator {chatMessage.DisplayName} said: \"{chatMessage.Message}\"");
                return true;
            }
            return false;
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
