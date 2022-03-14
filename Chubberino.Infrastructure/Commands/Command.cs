using System;
using System.Collections.Generic;
using System.IO;
using Chubberino.Common.ValueObjects;
using Chubberino.Infrastructure.Client.TwitchClients;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Infrastructure.Commands
{
    public abstract class Command : ICommand
    {
        public const String NoHelpImplementedMessage = "No help implemented yet.";

        public Name Name { get; init; }

        protected ITwitchClientManager TwitchClientManager { get; private set; }
        protected TextWriter Writer { get; }

        protected Command(ITwitchClientManager client, TextWriter writer)
        {
            TwitchClientManager = client;
            Writer = writer;
            Name = Name.From(GetType().Name.ToLowerInvariant());
            TwitchClientManager.OnTwitchClientRefreshed += TwitchClientManager_OnTwitchClientRefreshed;
        }

        private void TwitchClientManager_OnTwitchClientRefreshed(Object sender, OnTwitchClientRefreshedArgs e)
        {
            if (e.OldClient.HasValue)
            {
                Unregister(e.OldClient.Value);
            }
            Register(e.NewClient);
        }

        public abstract void Execute(IEnumerable<String> arguments);

        public virtual Boolean Set(String property, IEnumerable<String> arguments) { return false; }

        public virtual String Get(IEnumerable<String> arguments) { return null; }

        public virtual String GetHelp() => NoHelpImplementedMessage;

        public virtual Boolean Add(String property, IEnumerable<String> arguments) { return false; }

        public virtual Boolean Remove(String property, IEnumerable<String> arguments) { return false; }

        public virtual void Register(ITwitchClient client)
        {

        }

        public virtual void Unregister(ITwitchClient client)
        {

        }
    }
}
