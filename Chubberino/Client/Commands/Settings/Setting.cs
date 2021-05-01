using System;
using System.Collections.Generic;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    public abstract class Setting : Command, ISetting
    {
        private Boolean isEnabled;

        protected Action<ITwitchClient> Enable { get; set; } = client => { };

        protected Action<ITwitchClient> Disable { get; set; } = client => { };

        public Boolean IsEnabled
        {
            get => isEnabled;
            set
            {
                if (value)
                {
                    // Do not call Enable if already enabled, or multiple copies of the event will be added.
                    if (!isEnabled)
                    {
                        Enable(TwitchClientManager.Client);
                    }
                }
                else
                {
                    Disable(TwitchClientManager.Client);
                }

                isEnabled = value;
            }
        }

        public virtual String Status => IsEnabled ? "enabled" : "disabled";

        protected Setting(ITwitchClientManager client, IConsole console)
            : base(client, console)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            IsEnabled = !IsEnabled;
        }

        public void Refresh()
        {
            if (IsEnabled)
            {
                // Re-add events to new client.
                IsEnabled = false;
                IsEnabled = true;
            }
        }
    }
}
