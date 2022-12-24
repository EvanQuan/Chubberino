using Chubberino.Infrastructure.Credentials;

namespace Chubberino.Infrastructure.Commands;

public sealed class CommandConfigurationStrategy : ICommandConfigurationStrategy
{
    public void Configure(ICommandRepository repository, LoginCredentials loginCredentials)
    {
        if (loginCredentials.IsBot)
        {
            repository.Enable("cheese");
        }
    }
}
