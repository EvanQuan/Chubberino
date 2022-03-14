using Moq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    public sealed class WhenAddingCommand : UsingCommandRepository
    {
        [Fact]
        public void ShouldAddUserCommand()
        {
            Sut.AddCommand(MockedUserCommand1.Object);

            Assert.Single(Sut.Commands);
            Assert.Equal(MockedUserCommand1.Object, Sut.Commands[MockedUserCommand1.Object.Name]);

            Assert.Single(Sut.Settings.Disabled);
            Assert.Contains(MockedUserCommand1.Object, Sut.Settings.Disabled);
            Assert.Contains(MockedUserCommand1.Object, Sut.UserCommands.Values);
            Assert.DoesNotContain(MockedUserCommand1.Object, Sut.Settings.Enabled);
        }

        [Fact]
        public void ShouldAddCommand()
        {
            Sut.AddCommand(MockedUserCommand1.Object);

            Assert.Single(Sut.Commands);
            Assert.Equal(MockedUserCommand1.Object, Sut.Commands[MockedUserCommand1.Object.Name]);
        }
    }
}
