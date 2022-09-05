using Chubberino.Client.Commands.Settings.UserCommands;

namespace Chubberino.Infrastructure.Commands.Settings.UserCommands;

public interface IUserCommand : ISetting, IEventListenerCommand<OnUserCommandReceivedArgs>
{
}
