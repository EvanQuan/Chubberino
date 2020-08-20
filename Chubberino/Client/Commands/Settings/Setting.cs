using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    public abstract class Setting : Command, ISetting
    {
        private Boolean isEnabled;

        protected Action<ITwitchClient> Enable { get; set; }

        protected Action<ITwitchClient> Disable { get; set; }

        public Boolean IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                if (isEnabled)
                {
                    Enable(TwitchClient);
                }
                else
                {
                    Disable(TwitchClient);
                }
            }
        }

        public virtual String Status => IsEnabled ? "enabled" : "disabled";

        protected Setting(IExtendedClient client)
            : base(client)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            IsEnabled = !IsEnabled;
        }
    }
}
