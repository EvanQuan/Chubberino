﻿using System;
using System.Collections.Generic;

namespace Chubberino.Client.Commands
{
    public interface ICommand
    {
        String Name { get; }

        void Execute(IEnumerable<String> arguments);

        Boolean Set(String property, IEnumerable<String> arguments);

        Boolean Add(String property, IEnumerable<String> arguments);

        Boolean Remove(String property, IEnumerable<String> arguments);

        String Get(IEnumerable<String> arguments);

        /// <summary>
        /// Gets the help message explaining how to use the command.
        /// </summary>
        /// <returns>The help messag string.</returns>
        String GetHelp();
    }
}
