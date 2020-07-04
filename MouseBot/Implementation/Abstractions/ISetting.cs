using System;

namespace MouseBot.Implementation.Abstractions
{
    public interface ISetting : ICommand
    {
        Boolean IsEnabled { get; }

        String Status { get; }
    }
}
