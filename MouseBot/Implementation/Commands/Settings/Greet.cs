using MouseBot.Implementation.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands.Settings
{
    /// <summary>
    /// Greet a user immediately when they join the channel.
    /// </summary>
    internal sealed class Greet : Setting
    {
        /// <summary>
        /// Twitch sends the joined users in groups. To lessen the spam, the
        /// number of joined users to queue can be throttled.
        /// </summary>
        private Int32 GreetingLimit { get; set; }

        /// <summary>
        /// Greeting to add after the user name.
        /// </summary>
        private String Greeting { get; set; }

        public Greet(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnUserJoined += TwitchClient_OnUserJoined;
        }

        private void TwitchClient_OnUserJoined(Object sender, OnUserJoinedArgs e)
        {
            if (!IsEnabled) { return; }
            if (e.Username.Equals(TwitchInfo.BotUsername, StringComparison.OrdinalIgnoreCase)) { return; }

            if (Spooler.QueueSize < GreetingLimit)
            {
                Spooler.SpoolMessage($"@{e.Username} {Greeting}", Priority.Low);
            }
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            IsEnabled = arguments.Count() > 0;

            if (Int32.TryParse(arguments.FirstOrDefault(), out Int32 result))
            {
                GreetingLimit = result;
                Greeting = String.Join(" ", arguments.Skip(1));
            }
            else
            {
                GreetingLimit = 1;
                Greeting = String.Join(" ", arguments);
            }

            if (IsEnabled)
            {
                Console.WriteLine($"Greeting message is \"{Greeting}\" with limit of {GreetingLimit}.");
            }
        }
    }
}
