using Chubberino.Client.Abstractions;
using System;
using TwitchLib.Client.Exceptions;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client
{
    /// <summary>
    /// Queues messages, and sends them over timed intervals.
    /// </summary>
    public sealed class MessageSpooler : IMessageSpooler
    {
        public String ChannelName { get; private set; } = String.Empty;

        private String PreviousMessage { get; set; }
        public ITwitchClient TwitchClient { get; }

        public MessageSpooler(ITwitchClient client)
        {
            TwitchClient = client;
        }

        public void SetChannel(String channelName)
        {
            ChannelName = channelName;
        }

        private void SendMessage(String message)
        {
            if (message == PreviousMessage)
            {
                message += " " + Data.InvisibleCharacter;
            }

            try
            {
                TwitchClient.SendMessage(ChannelName, message);
                PreviousMessage = message;
            }
            catch (BadStateException e)
            {
                Console.WriteLine("ERROR: Failed to send message");
                Console.WriteLine(e.Message);
            }
        }

        public void SpoolMessage(String message)
        {
            SendMessage(message);
        }
    }
}
