using Chubberino.Client.Commands.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.TimeoutAlerts
{
    public abstract class UsingTimeoutAlert : UsingCommand
    {
        protected TimeoutAlert Sut { get; }

        public UsingTimeoutAlert()
        {
            Sut = new TimeoutAlert(MockedTwitchClient.Object, MockedConsole.Object);
        }
    }
}
