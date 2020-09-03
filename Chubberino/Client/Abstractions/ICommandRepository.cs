using System;
using System.Collections.Generic;

namespace Chubberino.Client.Abstractions
{
    public interface ICommandRepository
    {
        void RefreshAll(IExtendedClient twitchClient);

        String GetStatus();

        void Execute(String commandName, IEnumerable<String> arguments);

        void DisableAllSettings();
    }
}
