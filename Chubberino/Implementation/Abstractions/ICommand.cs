using System;
using System.Collections.Generic;
using System.Text;

namespace Chubberino.Implementation.Abstractions
{
    public interface ICommand
    {
        String Name { get; }

        void Execute(IEnumerable<String> arguments);

        Boolean Set(String value, IEnumerable<String> arguments);

        String Get(IEnumerable<String> arguments);
    }
}
