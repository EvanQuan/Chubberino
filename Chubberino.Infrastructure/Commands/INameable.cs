using System;
using Chubberino.Common.ValueObjects;

namespace Chubberino.Infrastructure.Commands
{
    public interface INameable
    {
        Name Name { get; }
    }
}
