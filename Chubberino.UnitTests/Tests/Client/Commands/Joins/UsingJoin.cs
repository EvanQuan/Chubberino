using Chubberino.Client;
using Chubberino.Client.Commands;
using TwitchLib.Communication.Models;

namespace Chubberino.UnitTests.Tests.Client.Commands.Joins
{
    public abstract class UsingJoin : UsingCommand
    {
        protected Join Sut { get; private set; }

        public UsingJoin()
        {
            Sut = new Join(MockedTwitchClient.Object, MockedConsole.Object);
        }
    }
}
