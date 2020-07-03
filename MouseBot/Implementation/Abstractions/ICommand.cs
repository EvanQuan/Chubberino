using System;
using System.Collections.Generic;
using System.Text;

namespace MouseBot.Implementation.Abstractions
{
    public interface ICommand
    {
        String Name { get; }

        void Execute(params String[] arguments);
    }
}
