using Chubberino.Client;
using Chubberino.Client.Commands.Settings.UserCommands;
using Moq;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Events;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.UserCommands
{
    public abstract class UsingUserCommand
    {
        protected Mock<ITwitchClientManager> MockedClient { get; }

        protected Mock<IConsole> MockedConsole { get; }

        protected UserCommandWrapper Sut { get; }

        protected UsingUserCommand()
        {
            MockedClient = new Mock<ITwitchClientManager>();
            MockedConsole = new Mock<IConsole>();

            Sut = new UserCommandWrapper(MockedClient.Object, MockedConsole.Object);
        }

        protected sealed class UserCommandWrapper : UserCommand
        {
            public UserCommandWrapper(ITwitchClientManager client, IConsole console) : base(client, console)
            {
            }

            public new Boolean TryValidateCommand(OnMessageReceivedArgs args, out IEnumerable<String> words) => base.TryValidateCommand(args, out words);
        }
    }
}
