using MouseBot.Implementation.Abstractions;
using MouseBot.Implementation.Commands;
using System;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands.Settings
{
    internal abstract class Setting : Command, ISetting
    {
        private Boolean isEnabled;

        public Boolean IsEnabled
        {
            get
            {
                return isEnabled;
            }
            protected set
            {
                isEnabled = value;
                Console.WriteLine($"{Name} {(isEnabled ? "enabled" : "disabled")}");
            }
        }

        protected Setting(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
        }

        public override void Execute(params String[] arguments)
        {
            IsEnabled = !IsEnabled;
        }
    }
}
