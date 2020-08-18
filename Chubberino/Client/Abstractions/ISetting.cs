using System;

namespace Chubberino.Client.Abstractions
{
    public interface ISetting : ICommand
    {
        Boolean IsEnabled { get; set; }

        String Status { get; }
    }
}
