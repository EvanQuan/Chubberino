using Chubberino.Client.Abstractions;
using System;
using System.Collections.Concurrent;
using TwitchLib.Client.Exceptions;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client
{
    /// <summary>
    /// Queues messages, and sends them over timed intervals.
    /// </summary>
    public sealed class MessageSpooler : Manager, IMessageSpooler
    {
        public String ChannelName { get; private set; } = String.Empty;

        private ConcurrentQueue<String> MessageQueue { get; set; }
            = new ConcurrentQueue<String>();

        private String PreviousMessage { get; set; }

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

        public Int32 QueueSize => MessageQueue.Count;

        public MessageSpooler(ITwitchClient client)
            : base(client)
        {
            Interval = TimeSpan.FromSeconds(5);
        }

        public void SetChannel(String channelName)
        {
            MessageQueue.Clear();
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
            //switch (priority)
            //{
            //case Priority.Low:
            //    break;
            //case Priority.High:
            //    if (MessageQueue.Count < MaximumQueueSize)
            //    {
            //        MessageQueue.Enqueue(message);
            //    }
            //    break;
            //}
        }

        /// <summary>
        /// Run repeat message task at interval.
        /// </summary>
        protected override void ManageTasks()
        {
            if (!String.IsNullOrEmpty(RepeatMessage))
            {
                SpoolMessage(RepeatMessage);
            }
        }
    }
}
