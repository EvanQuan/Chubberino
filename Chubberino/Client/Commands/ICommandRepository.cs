using Chubberino.Client.Commands.Settings;
using Chubberino.Client.Commands.Settings.UserCommands;
using System;
using System.Collections.Generic;

namespace Chubberino.Client.Commands
{
    public interface ICommandRepository
    {
        void RefreshAll();

        String GetStatus();

        /// <summary>
        /// Get all the <see cref="ISetting"/>s contained within <see cref="CommandList"/>.
        /// </summary>
        /// <returns>all the <see cref="ISetting"/>s contained within <see cref="CommandList"/>.</returns>
        IEnumerable<ISetting> Settings { get; }

        /// <summary>
        /// Get all the <see cref="IUserCommand"/>s contained within <see cref="CommandList"/>.
        /// </summary>
        /// <returns>all the <see cref="IUserCommand"/>s contained within <see cref="CommandList"/>.</returns>
        IEnumerable<IUserCommand> UserCommands { get; }

        ICommandRepository AddCommand(ICommand command);

        void Execute(String commandName, IEnumerable<String> arguments);

        void DisableAllSettings();

        void DisableAllUserCommands();

        void EnableAllUserCommands();
    }
}
