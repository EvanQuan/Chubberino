﻿using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
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
                        Enable(TwitchClient);
                    }
                }
                else
                {
                    Disable(TwitchClient);
                }

                isEnabled = value;
            }
        }

        public virtual String Status => IsEnabled ? "enabled" : "disabled";

        protected Setting(IExtendedClient client, TextWriter console)
            : base(client, console)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            IsEnabled = !IsEnabled;
        }

        public override void Refresh(IExtendedClient twitchClient)
        {
            if (IsEnabled)
            {
                IsEnabled = false;
                base.Refresh(twitchClient);
                IsEnabled = true;
            }
            else
            {
                base.Refresh(twitchClient);
            }
        }
    }
}
