using System;

namespace Chubberino.Implementation.Abstractions
{
    public interface ISetting : ICommand
    {
        Boolean IsEnabled { get; }

        String Status { get; }
    }
}
