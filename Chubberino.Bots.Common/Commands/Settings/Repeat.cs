using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings;
using TwitchLib.Client.Events;

namespace Chubberino.Bots.Common.Commands.Settings
{
    /// <summary>
    /// Repeat a specified message at the throttle limit.
    /// It is not recommended to sent messages manually while messages are
    /// being repeated from this, or you may incur a global IP shadow ban.
    /// </summary>
    public sealed class Repeat : Setting
    {
        public String RepeatMessage { get; set; }

        /// <summary>
        /// Indicates that we are waiting for the repeat message to be typed in chat.
        /// </summary>
        public Boolean WaitingForRepeatMessage { get; set; }

        private IRepeater Repeater { get; }

        public Repeat(ITwitchClientManager client, IRepeater repeater, TextWriter console)
            : base(client, console)
        {
            Repeater = repeater;
            Repeater.Action = SpoolRepeatMessages;
            Repeater.Interval = TimeSpan.FromSeconds(0.3);
        }

        private void SpoolRepeatMessages()
        {
            TwitchClientManager.SpoolMessage(RepeatMessage);
        }

        public override String Status => base.Status
            + $"\n\tMessage: {RepeatMessage}"
            + $"\n\tInterval: {Repeater.Interval.TotalSeconds} seconds"
            + $"\n\tVariance: {Repeater.Variance.TotalSeconds} seconds"
            + $"\n\tWait for repeat message: {WaitingForRepeatMessage}";

        public override void Execute(IEnumerable<String> arguments)
        {
            String proposedRepeatMessage = String.Join(" ", arguments);

            if (String.IsNullOrEmpty(proposedRepeatMessage))
            {
                UpdateState(SettingState.Toggle);
            }
            else
            {
                // Update the message and keep repeating.
                RepeatMessage = proposedRepeatMessage;
                UpdateState(SettingState.Enable);
            }

            Repeater.IsRunning = IsEnabled;
        }

        public override Boolean Set(String property, IEnumerable<String> arguments)
        {
            switch (property?.ToLower())
            {
                case "m":
                case "message":
                    RepeatMessage = String.Join(" ", arguments);
                    return true;
                case "i":
                case "interval":
                    {
                        if (Double.TryParse(arguments.FirstOrDefault(), out Double result))
                        {
                            Repeater.Interval = result >= 0
                                ? TimeSpan.FromSeconds(result)
                                : TimeSpan.Zero;

                            return true;
                        }
                    }
                    break;
                case "v":
                case "variance":
                    {
                        if (Double.TryParse(arguments.FirstOrDefault(), out Double result))
                        {
                            Repeater.Variance = result >= 0
                                ? TimeSpan.FromSeconds(result)
                                : TimeSpan.Zero;

                            return true;
                        }
                    }
                    break;
                case "w":
                case "wait":
                    if (!WaitingForRepeatMessage)
                    {
                        TwitchClientManager.Client.OnMessageReceived += TwitchClient_OnMessageReceived;
                        WaitingForRepeatMessage = true;
                    }
                    return true;
            }
            return false;
        }

        public void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!e.ChatMessage.Username.Equals(e.ChatMessage.BotUsername)) { return; }

            RepeatMessage = e.ChatMessage.Message;

            WaitingForRepeatMessage = false;
            TwitchClientManager.Client.OnMessageReceived -= TwitchClient_OnMessageReceived;
            Writer.WriteLine($"Received repeat message: \"{RepeatMessage}\"");
        }

        public override String GetHelp()
        {
            return @"
interval - the time between each message being sent

variance - the random range of time to add or subtract from each interval

wait - Indicates that we are waiting for the repeat message to be typed in chat.
       When true, this waits for the next message to be sent by the bot in
       chat and saves that as the repeat message.
       This is useful for messages that contain emojis or characters that
       otherwise cannot be probably encoded by typing them in the command line.
";
        }
    }
}
