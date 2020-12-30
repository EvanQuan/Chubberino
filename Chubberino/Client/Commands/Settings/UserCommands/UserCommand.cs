using Chubberino.Client.Abstractions;
using System.IO;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public abstract class UserCommand : Setting, IUserCommand
    {
        protected UserCommand(IExtendedClient client, TextWriter console) : base(client, console)
        {
        }
    }
}
