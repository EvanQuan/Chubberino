using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands
{
    public abstract class Command : ICommand
    {
        public String Name { get; }

        protected IExtendedClient TwitchClient { get; private set; }

        protected TextWriter Console { get; }

        protected Command(IExtendedClient client, TextWriter console)
        {
            TwitchClient = client;
            Console = console;
            Name = GetType().Name.ToLowerInvariant();
        }

        public abstract void Execute(IEnumerable<String> arguments);

        public virtual Boolean Set(String property, IEnumerable<String> arguments) { return false; }

        public virtual String Get(IEnumerable<String> arguments) { return null; }

        public virtual String GetHelp() { return "No help implemented yet."; }

        public virtual Boolean Add(String property, IEnumerable<String> arguments) { return false; }

        public virtual Boolean Remove(String property, IEnumerable<String> arguments) { return false; }

        /// <summary>
        /// Refresh the command with a new <see cref="ITwitchClient"/> and
        /// <see cref="IMessageSpooler"/>.
        /// </summary>
        /// <param name="twitchClient">New twitch client.</param>
        /// <param name="messageSpooler">New message spooler.</param>
        public void Refresh(IExtendedClient twitchClient)
        {
            TwitchClient = twitchClient;
        }
    }
}
