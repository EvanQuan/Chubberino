using Chubberino.Implementation.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Implementation.Commands.Settings
{
    /// <summary>
    /// Given a 
    /// </summary>
    internal sealed class Reply : Setting
    {
        private String ReplyMessage { get; set; }

        public override String Status => String.IsNullOrWhiteSpace(ReplyMessage)
            ? "disabled"
            : ReplyMessage;

        public Reply(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message == ReplyMessage)
            {
                TwitchClient.SendMessage(e.ChatMessage.Channel, $"@{e.ChatMessage.Username} {ReplyMessage}");
            }
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            IsEnabled = arguments.Count() > 0;

            if (IsEnabled)
            {
                ReplyMessage = String.Join(" ", arguments);
                Console.WriteLine($"Replying to \"{ReplyMessage}\"");
            }
            else
            {
                ReplyMessage = null;
                Console.WriteLine("Reply disabled");
            }

        }
    }
}
