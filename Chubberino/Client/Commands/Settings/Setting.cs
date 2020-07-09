using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    public abstract class Setting : Command, ISetting
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
