using System;

namespace Chubberino.Client.Abstractions
{
    public interface ISetting : ICommand
    {
        Boolean IsEnabled { get; }

        String Status { get; }
    }
}
