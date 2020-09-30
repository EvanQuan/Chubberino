using System;
using System.Collections.Generic;
using System.Text;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings
{
    public abstract class UsingSetting : UsingCommand
    {
        protected SettingExtender Sut { get; }

        public UsingSetting()
        {
            Sut = new SettingExtender(MockedTwitchClient.Object, MockedConsole.Object);
        }
    }
}
