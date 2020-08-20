using System;

namespace Chubberino.Client.Abstractions
{
    public interface IMessageSpooler
    {
        void SpoolMessage(String message);
    }
}
