using System;
using System.Collections.Generic;

namespace Chubberino.Client.Abstractions
{
    public interface ICommandRepository
    {
        IReadOnlyList<ICommand> Commands { get; }

        void RefreshAll(IExtendedClient twitchClient);

        String GetStatus();

        IEnumerable<ISetting> GetSettings();

        void Execute(String commandName, IEnumerable<String> arguments);

        void DisableAllSettings();
    }
}
