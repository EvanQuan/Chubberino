using Chubberino.Client.Commands.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.UsingGreet
{
    public abstract class GreetTestBase : CommandTestBase
    {
        protected Greet Sut { get; }

        public GreetTestBase()
        {
            Sut = new Greet(MockedTwitchClient.Object, MockedSpooler.Object);
        }
    }
}
