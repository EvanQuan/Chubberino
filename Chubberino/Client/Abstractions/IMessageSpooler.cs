using System;

namespace Chubberino.Client.Abstractions
{
    public interface IMessageSpooler
    {
        String ChannelName { get; }

        void SetChannel(String channelName);

        void SpoolMessage(String message);
    }
}
