using System;

namespace Chubberino.Client.Abstractions
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
