using System;

namespace Chubberino.Client.Abstractions
{
    public interface IMessageSpooler
    {
        String ChannelName { get; }

        String RepeatMessage { get; set; }

        TimeSpan Interval { get; set; }

        Int32 QueueSize { get; }

        void SetChannel(String channelName);

        void SpoolMessage(String message, Priority priority);

        void Start();
    }
}
