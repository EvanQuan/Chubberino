using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;

namespace Chubberino.Client.Commands
{
    public abstract class Command : ICommand
    {
        public String Name { get; }

        protected ITwitchClientManager TwitchClientManager { get; private set; }

        protected IConsole Console { get; }

        protected Command(ITwitchClientManager client, IConsole console)
        {
            TwitchClientManager = client;
            Console = console;
            Name = GetType().Name.ToLowerInvariant();
        }

        public abstract void Execute(IEnumerable<String> arguments);

        public virtual Boolean Set(String property, IEnumerable<String> arguments) { return false; }

        public virtual String Get(IEnumerable<String> arguments) { return null; }

        public virtual String GetHelp() { return "No help implemented yet."; }

        public virtual Boolean Add(String property, IEnumerable<String> arguments) { return false; }

        public virtual Boolean Remove(String property, IEnumerable<String> arguments) { return false; }
    }
}
