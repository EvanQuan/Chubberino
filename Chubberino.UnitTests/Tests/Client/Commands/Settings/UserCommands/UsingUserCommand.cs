using Chubberino.Infrastructure.Commands.Settings.UserCommands;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.UserCommands
{
    public abstract class UsingUserCommand
    {
        protected UserCommandValidator Sut { get; }

        protected UsingUserCommand()
        {
            Sut = new();
        }
    }
}
