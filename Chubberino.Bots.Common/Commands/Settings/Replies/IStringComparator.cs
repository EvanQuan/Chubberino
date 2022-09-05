using System;
using Chubberino.Common.ValueObjects;

namespace Chubberino.Bots.Common.Commands.Settings.Replies;

public interface IStringComparator
{
    Name Name { get; }

    Boolean Matches(String left, String right);
}
