using MouseBot.Implementation.Abstractions;
using System;
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
            if (IsEnabled)
            {
                Spooler.SpoolMessage($"@{e.Username} {Greeting}");
            }
        }

        public override void Execute(params String[] arguments)
        {
            IsEnabled = arguments.Length > 0;

            Greeting = String.Join(" ", arguments);
        }
    }
}
