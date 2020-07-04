using MouseBot.Implementation.Abstractions;
using MouseBot.Implementation.Commands;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands.Settings
{
    internal abstract class Setting : Command, ISetting
    {
        public Boolean IsEnabled { get; protected set; }

        public virtual String Status => IsEnabled ? "enabled" : "disabled";

        protected Setting(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            IsEnabled = !IsEnabled;
        }
    }
}
