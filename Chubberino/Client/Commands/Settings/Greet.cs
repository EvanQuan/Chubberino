using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    /// <summary>
    /// Greet a user immediately when they join the channel.
    /// </summary>
    public sealed class Greet : Setting
    {
        public enum Mode
        {
            Default = 0,
            Wholesome = 1,
        }

        public Mode CurrentMode { get; set; }

        /// <summary>
        /// Greeting to add after the user name.
        /// </summary>
        private String Greeting { get; set; }

        private ComplimentGenerator Compliments { get; }

        public override String Status => (IsEnabled
            ? $"\"{Greeting}\""
            : "disabled")
            + $" Mode: {CurrentMode}";

        public Greet(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnUserJoined += TwitchClient_OnUserJoined;
            Compliments = new ComplimentGenerator();
        }

        private void TwitchClient_OnUserJoined(Object sender, OnUserJoinedArgs e)
        {
            if (!IsEnabled) { return; }

            // Don't count self
            if (e.Username.Equals(TwitchInfo.BotUsername, StringComparison.OrdinalIgnoreCase)) { return; }

            Spooler.SpoolMessage($"@{e.Username} {Greeting} {(CurrentMode == Mode.Wholesome ? Compliments.GetCompliment() : String.Empty)}");
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            IsEnabled = arguments.Count() > 0;

            if (IsEnabled)
            {
                Greeting = String.Join(" ", arguments);
                Console.WriteLine($"Greeting message is \"{Greeting}\".");
            }
            else
            {
                Greeting = null;
            }
        }

        public override Boolean Set(String property, IEnumerable<String> arguments)
        {
            switch (property.ToLower())
            {
                case "m":
                case "mode":
                {
                    switch (arguments.FirstOrDefault()?.ToLower())
                    {
                        case "w":
                        case "wholesome":
                                CurrentMode = Mode.Wholesome;
                            break;
                        default:
                                CurrentMode = Mode.Default;
                            break;
                    }
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
Message users when they join the channel. Twitch sends this information in
bunches, so this is not very effective at announcing when a user has joined the
channel, but instead for tagging random viewers.

usage: greet <message>

    <message>   The message to append to the username greeted.

set:
    mode    default - No special effect
            wholesome - Appends a random compliment and emote
";
        }
    }
}
