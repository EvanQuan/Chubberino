using Chubberino.Infrastructure.Credentials;

namespace Chubberino.Infrastructure.Commands
{
    public interface ICommandConfigurationStrategy
    {
        void Configure(ICommandRepository repository, LoginCredentials loginCredentials);
    }
}
