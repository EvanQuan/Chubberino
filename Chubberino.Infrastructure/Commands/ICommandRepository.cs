using System;
using System.Collections.Generic;
using Chubberino.Common.ValueObjects;
using Chubberino.Infrastructure.Commands.Settings;
using Chubberino.Infrastructure.Credentials;

namespace Chubberino.Infrastructure.Commands;

public interface ICommandRepository
{
    String GetStatus();

    ICommandRepository AddCommand(ICommand command);
    ICommandRepository AddCommand(ISetting command, Boolean enabled = false);

    void Execute(Name commandName, IEnumerable<String> arguments);

    void DisableAllSettings();

    void DisableAllUserCommands();

    ICommandRepository Disable(Name settingName);

    ICommandRepository Enable(Name settingName);

    void Configure(LoginCredentials loginCredentials);
}
