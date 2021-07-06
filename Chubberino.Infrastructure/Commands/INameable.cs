using System;

namespace Chubberino.Infrastructure.Commands
{
    public interface INameable
    {
        [Obsolete("Replace with Lowercase String")]
        String Name { get; }
    }
}
