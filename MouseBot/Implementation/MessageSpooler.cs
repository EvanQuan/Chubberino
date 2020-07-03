using MouseBot.Implementation.Abstractions;
using System;
using TwitchLib.Client.Exceptions;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation
{
    /// <summary>
    /// Queues messages, and sends them over timed intervals.
    /// </summary>
    public sealed class MessageSpooler : Manager, IMessageSpooler
    {
        /// <summary>
        /// Empty character to evade identical message detection.
        /// </summary>
        private const String EmptyCharacter = "⠀";

        public String ChannelName { get; private set; }

        private String LastMessage { get; set; }

        private TimedQueue MessageQueue { get; set; } = new TimedQueue();

        private String repeatMessage;

        public String RepeatMessage
        {
            get => repeatMessage;
            set
            {
                MessageQueue.Clear();
                repeatMessage = value;
                Console.WriteLine(repeatMessage is null
                    ? "Repeat disabled."
                    : $"Repeating \"{repeatMessage}\"");
            }
        }

        public TimeSpan Interval
        {
            get => MessageQueue.Interval;
            set => MessageQueue.Interval = value;
        }

        public MessageSpooler(ITwitchClient client)
            : base(client)
        {

        }

        public void SetChannel(String channelName)
        {
            MessageQueue.Clear();
            ChannelName = channelName;
        }

        private void SendMessage(String message)
        {
            if (!TwitchClient.IsConnected)
            {
                TwitchClient.Connect();

            }

            if (TwitchClient.JoinedChannels.Count == 0)
            {
                TwitchClient.JoinChannel(ChannelName);
            }

            if (message == LastMessage)
            {
                message += " " + EmptyCharacter;
            }

            try
            {
                TwitchClient.SendMessage(ChannelName, message);
            }
            catch (BadStateException e)
            {
                Console.WriteLine("ERROR: Failed to send message");
                Console.WriteLine(e.Message);
            }
            LastMessage = message;
        }

        public void SpoolMessage(String message)
        {
            MessageQueue.Enqueue(message);
        }

        protected override void ManageTasks()
        {
            if (MessageQueue.Count == 0)
            {
                if (repeatMessage != null)
                {
                    SpoolMessage(repeatMessage);
                }
            }
            if (MessageQueue.TryDequeue(out String message))
            {
                SendMessage(message);
            }
        }
    }
}
