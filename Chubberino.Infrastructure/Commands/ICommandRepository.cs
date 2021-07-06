using System;
using System.Collections.Generic;
using Chubberino.Infrastructure.Credentials;

namespace Chubberino.Infrastructure.Commands
{
    public interface ICommandRepository
    {
        String GetStatus();

        ICommandRepository AddCommand(ICommand command);

        void Execute(String commandName, IEnumerable<String> arguments);

        void DisableAllSettings();

        void DisableAllUserCommands();

        ICommandRepository Disable(String settingName);

        ICommandRepository Enable(String settingName);

        void Configure(LoginCredentials loginCredentials);
    }
}
