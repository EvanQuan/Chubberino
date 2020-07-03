using MouseBot.Implementation.Abstractions;
using System;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands
{
    public abstract class Command : ICommand
    {
        public String Name { get; }

        protected ITwitchClient TwitchClient { get; private set; }

        protected IMessageSpooler Spooler { get; private set; }

        protected Command(ITwitchClient client, IMessageSpooler spooler)
        {
            TwitchClient = client;
            Spooler = spooler;
            Name = GetType().Name.ToLowerInvariant();
        }

        public abstract void Execute(params String[] arguments);
    }
}
