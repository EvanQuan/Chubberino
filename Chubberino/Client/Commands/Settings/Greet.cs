using Chubberino.Client.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
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
        private TimeSpan Cooldown { get; set; }

        /// <summary>
        /// Greeting to add after the user name.
        /// </summary>
        private String Greeting { get; set; }

        private ConcurrentQueue<String> GreetingLimit { get; }

        public override String Status => IsEnabled
            ? $"\"{Greeting}\" Cooldown: {Cooldown.TotalSeconds} seconds"
            : "disabled";

        public Greet(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnUserJoined += TwitchClient_OnUserJoined;
            GreetingLimit = new ConcurrentQueue<String>();
        }

        private void TwitchClient_OnUserJoined(Object sender, OnUserJoinedArgs e)
        {
            if (!IsEnabled) { return; }

            // Don't count self
            if (e.Username.Equals(TwitchInfo.BotUsername, StringComparison.OrdinalIgnoreCase)) { return; }


            // if (GreetingLimit.Count < 5)
            //if (GreetingLimit.Count < 5)
            //{
            //GreetingLimit.Enqueue(e.Username);
            Spooler.SpoolMessage($"@{e.Username} {Greeting}", Priority.Low);
            //if (GreetingLimit.TryDequeue(out String result))
            //{
            //    Spooler.SpoolMessage($"@{String.Join("", result.Reverse())} {Greeting}", Priority.Low);
            //}
            //}

        }

        public override void Execute(IEnumerable<String> arguments)
        {
            IsEnabled = arguments.Count() > 0;

            if (IsEnabled)
            {
                Greeting = String.Join(" ", arguments);
                Console.WriteLine($"Greeting message is \"{Greeting}\" with cooldown of {Cooldown.TotalSeconds} seconds.");
            }
            else
            {
                Greeting = null;
            }
        }
    }
}
