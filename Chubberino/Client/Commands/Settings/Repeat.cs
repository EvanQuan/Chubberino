using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.Automation;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    /// <summary>
    /// Repeat a specified message at the throttle limit.
    /// It is not recommended to sent messages manually while messages are
    /// being repeated from this, or you may incur a global IP shadow ban.
    /// </summary>
    public sealed class Repeat : Setting
    {
        private String RepeatMessage { get; set; }

        private IRepeater Repeater { get; }

        private IStopSettingStrategy StopSettingStrategy { get; }

        public Repeat(ITwitchClient client, IMessageSpooler spooler, IRepeater repeater, IStopSettingStrategy stopSettingStrategy)
            : base(client, spooler)
        {
            Repeater = repeater;
            Repeater.Action = SpoolRepeatMessages;
            Repeater.Interval = TimeSpan.FromSeconds(1.8);
            StopSettingStrategy = stopSettingStrategy;
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
        }

        private void SpoolRepeatMessages()
        {
            Spooler.SpoolMessage(RepeatMessage);
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!IsEnabled) { return; }

            if (StopSettingStrategy.ShouldStop(e.ChatMessage))
            {
                IsEnabled = false;
                Repeater.IsRunning = false;
                Console.WriteLine("! ! ! STOPPED REPEAT ! ! !");
                Console.WriteLine($"Moderator {e.ChatMessage.DisplayName} said: \"{e.ChatMessage.Message}\"");
            }
        }

        public override String Status => (String.IsNullOrWhiteSpace(RepeatMessage)
            ? "disabled"
            : RepeatMessage)
            + $"\n\tInterval: {Repeater.Interval.TotalSeconds} seconds"
            + $"\n\tVariance: {Repeater.Variance.TotalSeconds} seconds";

        public override void Execute(IEnumerable<String> arguments)
        {
            String proposedRepeatMessage = String.Join(" ", arguments);

            if (String.IsNullOrEmpty(proposedRepeatMessage))
            {
                // No arguments toggles.
                IsEnabled = !IsEnabled;
            }
            else
            {
                // Update the message and keep repeating.
                RepeatMessage = proposedRepeatMessage;
                IsEnabled = true;
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
            }
            return false;
        }

        public override String GetHelp()
        {
            return @"
It is recommended to not go below 1.5 seconds or type any other messages
manually, or have other settings enabled for short intervals  to avoid a global
shadow ban.
";
        }
    }
}
