using System;

namespace Chubberino.Client.Abstractions
{
    public interface IMessageSpooler
    {
        String ChannelName { get; }

        String RepeatMessage { get; set; }

        TimeSpan Interval { get; set; }

        void SetChannel(String channelName);

        void SpoolMessage(String message);

        void Start();
    }
}
