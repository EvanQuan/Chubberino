using System;

namespace Chubberino.Client.Commands.Settings
{
    public interface ISetting : ICommand
    {
        Boolean IsEnabled { get; set; }

        String Status { get; }


        /// <summary>
        /// Refresh the command.
        /// </summary>
        void Refresh();
    }
}
